using System;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용한 RESTWorker 추상 클래스
    /// </summary>
    public abstract class UnityRESTWorker : UnityNetworker, IRESTWorker
    {
        // 수신 데이터에 ResponseHeaders 포함 여부
        private readonly bool includeResponseHeaders = false;
        /// <summary>
        /// 생성자
        /// </summary>
        public UnityRESTWorker()
        {

        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="includeResponseHeaders">수신 데이터에 ResponseHeaders 포함 여부</param>
        public UnityRESTWorker(bool includeResponseHeaders)
        {
            this.includeResponseHeaders = includeResponseHeaders;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public override void Dispose()
        {
            callback = null;
        }

        /// <summary>
        /// REST API 요청
        /// </summary>
        public abstract IRESTWorker Request(IRESTReceipt receipt);

        // REST API 수신 콜백
        private Action<IResponse<string>> callback = null;

        /// <summary>
        /// REST API 수신 콜백 등록
        /// </summary>
        public virtual IRESTWorker Subscribe(Action<IResponse<string>> callback)
        {
            this.callback = callback;

            return this;
        }

        /// <summary>
        /// 옵저버에 노티 (UnityWebRequest 수신 데이터를 기반으로 수신 데이터 재구성)
        /// </summary>
        /// <param name="req">UnityWebRequest</param>
        protected virtual void Notify(UnityWebRequest req)
        {
            if (callback != null)
            {
                IResponse<string> res = MakeResponse(req);
                callback.Invoke(res);
            }
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// </summary>
        private IResponse<string> MakeResponse(UnityWebRequest target)
        {
            if (includeResponseHeaders)
                return new Response<string>(target.downloadHandler.text, target.responseCode, target.error, ToRESTResult(target.result), target.GetResponseHeaders());
            else
                return new Response<string>(target.downloadHandler.text, target.responseCode, target.error, ToRESTResult(target.result));
        }

        /// <summary>
        /// IRESTReceipt 형식의 REST 접수증을 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        protected UnityWebRequest MakeUnityWebRequest(IRESTReceipt receipt)
        {
            UnityWebRequest req = MakeUnityWebRequest(receipt.method, receipt.uri, receipt.body);
            SetTimeout(req, receipt.timeout);
            SetRequestHeaders(req, receipt.headers);

            return req;
        }

        /// <summary>
        /// 필요 정보를 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="uri">Uri</param>
        /// <param name="body">Post data</param>
        /// <returns>생성한 UnityWebRequest</returns>
        protected virtual UnityWebRequest MakeUnityWebRequest(NetworkMethod method, Uri uri, string body)
        {
            switch (method)
            {
                case NetworkMethod.GET:
                    return UnityWebRequest.Get(uri);
                case NetworkMethod.POST:
                    return UnityWebRequest.Post(uri, body, "application/json");
                case NetworkMethod.PATCH:
                    return UnityWebRequest.Put(uri, body);
                case NetworkMethod.DELETE:
                    return UnityWebRequest.Delete(uri);
                default:
                    return null;
            }
        }
    }
}