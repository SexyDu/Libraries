using System;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용한 Byte array 다운로드 기반 클래스
    /// </summary>
    public abstract class UnityBytesDownloader : UnityNetworker, IBytesSubject
    {
        // 수신 콜백
        protected Action<IBytesResponse> callback = null;

        /// <summary>
        /// 수신 콜백 등록
        /// </summary>
        public virtual IBytesSubject Subscribe(Action<IBytesResponse> callback)
        {
            this.callback = callback;

            return this;
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// </summary>
        protected BytesResponse MakeResponse(UnityWebRequest target)
        {
            return new BytesResponse(target.downloadHandler.data, target.responseCode, target.error, ToRESTResult(target.result));
        }
    }
}