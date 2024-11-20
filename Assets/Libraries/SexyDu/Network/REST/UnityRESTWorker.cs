using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용한 RESTWorker 추상 클래스
    /// </summary>
    public abstract class UnityRESTWorker : IRESTWorker
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

        // REST API 수신 콜백
        protected Action<IRESTResponse> callback = null;

        /// <summary>
        /// REST API 수신 콜백 등록
        /// </summary>
        public virtual IRESTWorker Subscribe(Action<IRESTResponse> callback)
        {
            this.callback = callback;

            return this;
        }

        /// <summary>
        /// 타임아웃 설정
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeout"></param>
        protected void SetTimeout(UnityWebRequest target, int timeout)
        {
            if (timeout > 0)
                target.timeout = timeout;
        }

        /// <summary>
        /// 요청 헤더 설정
        /// </summary>
        protected void SetRequestHeaders(UnityWebRequest target, Dictionary<string, string> headers)
        {
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    target.SetRequestHeader(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// </summary>
        protected RESTResponse MakeRESTResponse(UnityWebRequest target)
        {
            if (includeResponseHeaders)
                return new RESTResponse(target.responseCode, target.downloadHandler.text, target.error, ToRESTResult(target.result), target.GetResponseHeaders());
            else
                return new RESTResponse(target.responseCode, target.downloadHandler.text, target.error, ToRESTResult(target.result));
        }

        /// <summary>
        /// UnityWebRequest.Result를 RESTResult 형식으로 변환
        /// </summary>
        private RESTResult ToRESTResult(UnityWebRequest.Result result)
        {
            switch (result)
            {
                case UnityWebRequest.Result.InProgress:
                    return RESTResult.InProgress;
                case UnityWebRequest.Result.Success:
                    return RESTResult.Success;
                case UnityWebRequest.Result.ConnectionError:
                    return RESTResult.ConnectionError;
                case UnityWebRequest.Result.ProtocolError:
                    return RESTResult.ProtocolError;
                case UnityWebRequest.Result.DataProcessingError:
                    return RESTResult.DataProcessingError;
                default:
                    return RESTResult.Unknown;
            }
        }

        /// <summary>
        /// IRESTReceipt 형식의 REST 접수증을 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        protected UnityWebRequest MakeUnityWebRequest(IRESTReceipt receipt)
        {
            return MakeUnityWebRequest(receipt.method, receipt.uri, string.Empty);
        }
        /// <summary>
        /// IPostableRESTReceipt 형식의 REST 접수증을 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        protected UnityWebRequest MakeUnityWebRequest(IPostableRESTReceipt receipt)
        {
            return MakeUnityWebRequest(receipt.method, receipt.uri, receipt.body);
        }
        /// <summary>
        /// 필요 정보를 받아 UnityWebRequest를 생성하여 반환
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="uri">Uri</param>
        /// <param name="body">Post data</param>
        /// <returns>생성한 UnityWebRequest</returns>
        private UnityWebRequest MakeUnityWebRequest(RESTMethod method, Uri uri, string body)
        {
            switch (method)
            {
                case RESTMethod.GET:
                    return UnityWebRequest.Get(uri);
                case RESTMethod.POST:
                    return UnityWebRequest.Post(uri, body, "application/json");
                case RESTMethod.PATCH:
                    return UnityWebRequest.Put(uri, body);
                case RESTMethod.DELETE:
                    return UnityWebRequest.Delete(uri);
                default:
                    return null;
            }
        }
    }
}