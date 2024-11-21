using System;
using UnityEngine;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용한 Texture(2D) 다운로드 기반 클래스
    /// </summary>
    public class UnityTextureDownloader : UnityNetworker, ITextureSubject
    {
        // 수신 콜백
        protected Action<ITextureResponse> callback = null;

        /// <summary>
        /// 수신 콜백 등록
        /// </summary>
        public virtual ITextureSubject Subscribe(Action<ITextureResponse> callback)
        {
            this.callback = callback;

            return this;
        }

        /// <summary>
        /// 접수증을 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        protected UnityWebRequest MakeUnityWebRequest(IBinaryReceipt receipt)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(receipt.uri);
            SetTimeout(req, receipt.timeout);
            return req;
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// </summary>
        protected TextureResponse MakeResponse(UnityWebRequest target)
        {
            return new TextureResponse(DownloadHandlerTexture.GetContent(target), target.responseCode, target.error, ToRESTResult(target.result));
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