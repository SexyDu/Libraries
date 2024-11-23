using System;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용한 Byte array 다운로드 기반 클래스
    /// </summary>
    public abstract class UnityBytesDownloader : UnityNetworker, IBytesDownloader
    {
        /// <summary>
        /// 접수증을 받아 다운로드 작업을 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>작업자</returns>
        public abstract IBytesDownloader Request(IBinaryReceipt receipt);

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
        /// 접수증을 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        protected virtual UnityWebRequest MakeUnityWebRequest(IBinaryReceipt receipt)
        {
            UnityWebRequest req = UnityWebRequest.Get(receipt.uri);
            SetTimeout(req, receipt.timeout);
            return req;
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