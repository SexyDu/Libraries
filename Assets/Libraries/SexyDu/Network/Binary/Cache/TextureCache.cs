using System;
using UnityEngine;

namespace SexyDu.Network
{
    /// <summary>
    /// 텍스쳐 캐시
    /// </summary>
    public class TextureCache : BinaryCache, ITextureDownloader
    {
        // 텍스쳐 콜백
        private Action<ITextureResponse> callback = null;

        /// <summary>
        /// 요청 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>다운로더 인터페이스</returns>
        public ITextureDownloader Request(IBinaryReceipt receipt)
        {
            new SexyBytesDownloader().Request(receipt).Subscribe(res =>
            {
                Texture2D tex2D = ByteArrayToTexture2D(res.data);

                callback?.Invoke(new TextureResponse(tex2D, res.code, res.error, res.result));
            });

            return this;
        }

        /// <summary>
        /// 콜백 등록
        /// </summary>
        /// <param name="callback">콜백</param>
        /// <returns>옵저거 서브젝트 인터페이스</returns>
        public ITextureSubject Subscribe(Action<ITextureResponse> callback)
        {
            this.callback = callback;

            return this;
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