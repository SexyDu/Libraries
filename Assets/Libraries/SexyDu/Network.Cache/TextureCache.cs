using System;
using SexyDu.Tool;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 텍스쳐 캐시
    /// </summary>
    public class TextureCache : BinaryCache//, ITextureDownloader
    {
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
        public virtual TextureCache Request(ICacheReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");

            worker = MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }

        // 텍스쳐 콜백
        private Action<ITextureResponse> callback = null;
        /// <summary>
        /// 콜백 등록
        /// </summary>
        /// <param name="callback">콜백</param>
        /// <returns>옵저거 서브젝트 인터페이스</returns>
        public TextureCache Subscribe(Action<ITextureResponse> callback)
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