#define USE_COROUTINE

using System;
using System.Collections;
using System.IO;
using SexyDu.Tool;
using UnityEngine;

namespace SexyDu.Network
{
    /// <summary>
    /// 암호화 텍스쳐 캐시
    /// </summary>
    public class TextureEncryptedCache : EncryptedBinaryCache, ITextureDownloader
    {
        public TextureEncryptedCache() { }

        /// <summary>
        /// 암호화 키 및 초기화 벡터 설정
        /// </summary>
        /// <param name="key">암호화 키(base64 44 length = 32byte = 256bit)</param>
        /// <param name="iv">초기화 벡터(base64 24 length = 16byte = 128bit)</param>
        public TextureEncryptedCache(char[] key, char[] iv) : base(key, iv) { }

        public override void Dispose()
        {
            base.Dispose();

            callback = null;
        }

        /// <summary>
        /// 요청 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>다운로더 인터페이스</returns>
        public virtual ITextureDownloader Request(IBinaryReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");
#if USE_COROUTINE
            worker = MonoHelper.StartCoroutine(CoRequest(receipt));
#else
            // uri 기반의 캐시 경로 가져오기
            string filePath = GetCachePath(receipt.uri.AbsoluteUri);

            // 캐시 파일이 존재하는 경우
            if (File.Exists(filePath))
            {
                // 캐시 파일 읽은 후 옵저버에 노티
                worker = MonoHelper.StartCoroutine(CoReadFileAndNotify(filePath));
            }
            // 캐시 파일이 존재하지 않는 경우
            else
            {
                // 다운로더 요청 후 캐시 파일 쓰기 및 옵저버에 노티
                worker = MakeDownloader().Request(receipt).Subscribe(res =>
                {
                    MonoHelper.StartCoroutine(CoWriteFile(filePath, res.data));

                    // 옵저버에 노티
                    Notify(res.data, res.code, res.error, res.result);
                });
            }
#endif
            return this;
        }

#if USE_COROUTINE
        private IEnumerator CoRequest(IBinaryReceipt receipt)
        {
            // uri 기반의 캐시 경로 가져오기
            string filePath = GetCachePath(receipt.uri.AbsoluteUri);

            // 캐시 파일이 존재하는 경우
            if (File.Exists(filePath))
            {
                // 캐시 파일 읽은 후 옵저버에 노티
                yield return CoReadFileAndNotify(filePath);
            }
            // 캐시 파일이 존재하지 않는 경우
            else
            {
                byte[] responseData = null;

                // 다운로더 요청 후 캐시 파일 쓰기 및 옵저버에 노티
                INetworker worker = MakeDownloader().Request(receipt).Subscribe(res =>
                {
                    responseData = res.data;

                    // 옵저버에 노티
                    Notify(res.data, res.code, res.error, res.result);
                });
                // 작업 완료 대기
                yield return new WaitUntil(() => !worker.IsWorking);

                // 캐시 파일 쓰기
                yield return CoWriteFile(filePath, responseData);
            }

            Terminate();
        }
#endif

        // 텍스쳐 콜백
        private Action<ITextureResponse> callback = null;
        /// <summary>
        /// 콜백 등록
        /// </summary>
        /// <param name="callback">콜백</param>
        /// <returns>옵저거 서브젝트 인터페이스</returns>
        public ITextureDownloader Subscribe(Action<ITextureResponse> callback)
        {
            this.callback = callback;

            return this;
        }
        /// <summary>
        /// 옵저버에 노티 (IBytesResponse 수신 데이터를 기반으로 수신 데이터 재구성)
        /// </summary>
        /// <param name="res">bytes 수신 데이터</param>
        protected override void Notify(byte[] data, long code, string error, NetworkResult result)
        {
            if (callback != null)
            {
                Texture2D tex2D = ByteArrayToTexture2D(data);
                callback.Invoke(new TextureResponse(tex2D, code, error, result));
            }
        }

        /// <summary>
        /// byte array를 받아 Texture2D로 변환하는 함수
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <returns>Texture2D</returns>
        private Texture2D ByteArrayToTexture2D(byte[] bytes)
        {
            // 바이트가 없는 경우 null반환
            if (bytes == null)
                return null;
            // 바이트가 있는 경우
            else
            {
                // 텍스쳐2D 생성 및 속성 설정
                Texture2D tex2D = new Texture2D(0, 0);
                tex2D.wrapMode = TextureWrapMode.Clamp;
                tex2D.filterMode = FilterMode.Bilinear;

                // byte 이미지 변환
                tex2D.LoadImage(bytes);

                // 이미지 반환
                return tex2D;
            }
        }
    }
}