using System;
using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;
using System.Collections.Generic;

namespace SexyDu.Network
{
    public interface IRESTSubject
    {
        public IRESTSubject Subscribe(Action<IRESTResponse> callback);
    }

    public struct UnityPostableRESTReceipt : IPostableRESTReceipt
    {
        public Uri uri
        {
            get;
            private set;
        }

        public RESTMethod method
        {
            get;
            private set;
        }

        public int timeout
        {
            get;
            private set;
        }

        public Dictionary<string, string> headers
        {
            get;
            private set;
        }

        public string body
        {
            get;
            private set;
        }

        public UnityPostableRESTReceipt(RESTMethod method)
        {
            this.method = method;

            uri = null;
            timeout = 0;
            headers = null;
            body = string.Empty;
        }

        public UnityPostableRESTReceipt SetUri(string url)
        {
            this.uri = new Uri(url);
            return this;
        }
        public UnityPostableRESTReceipt SetMethod(RESTMethod method)
        {
            this.method = method;
            return this;
        }
        public UnityPostableRESTReceipt SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }
        public UnityPostableRESTReceipt SetHeaders(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }
        public UnityPostableRESTReceipt AddHeader(string key, string value)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();
            headers.Add(key, value);

            return this;
        }
        public UnityPostableRESTReceipt SetBody(string body)
        {
            this.body = body;
            return this;
        }

        public UnityWebRequest GetUnityWebRequest()
        {
            UnityWebRequest req = null;
            switch (method)
            {
                case RESTMethod.GET:
                    req = UnityWebRequest.Get(uri);
                    break;
                case RESTMethod.POST:
                    req = UnityWebRequest.Post(uri, body, "application/json");
                    break;
                case RESTMethod.PATCH:
                    req = UnityWebRequest.Put(uri, body);
                    break;
                case RESTMethod.DELETE:
                    req = UnityWebRequest.Delete(uri);
                    break;
                default:
                    return null;
            }

            req.method = ToMethodString(method);

            return req;
        }

        private string ToMethodString(RESTMethod method)
        {
            switch (method)
            {
                case RESTMethod.GET:
                    return "GET";
                case RESTMethod.POST:
                    return "POST";
                case RESTMethod.PATCH:
                    return "PATCH";
                case RESTMethod.DELETE:
                    return "DELETE";
                default:
                    return string.Empty;
            }
        }
    }

    public struct UnityRESTReceipt : IRESTReceipt
    {
        public Uri uri
        {
            get;
            private set;
        }

        public RESTMethod method
        {
            get;
            private set;
        }

        public int timeout
        {
            get;
            private set;
        }

        public Dictionary<string, string> headers
        {
            get;
            private set;
        }

        public UnityRESTReceipt(RESTMethod method)
        {
            this.method = method;

            uri = null;
            timeout = 0;
            headers = null;
        }

        public UnityRESTReceipt SetUri(string url)
        {
            this.uri = new Uri(url);
            return this;
        }
        public UnityRESTReceipt SetMethod(RESTMethod method)
        {
            this.method = method;
            return this;
        }
        public UnityRESTReceipt SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }
        public UnityRESTReceipt SetHeaders(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }
        public UnityRESTReceipt AddHeader(string key, string value)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();
            headers.Add(key, value);

            return this;
        }

        public UnityWebRequest GetUnityWebRequest()
        {
            UnityWebRequest req = null;
            switch (method)
            {
                case RESTMethod.GET:
                    req = UnityWebRequest.Get(uri);
                    break;
                case RESTMethod.POST:
                    req = UnityWebRequest.Post(uri, string.Empty, "application/json");
                    break;
                case RESTMethod.PATCH:
                    req = UnityWebRequest.Put(uri, string.Empty);
                    break;
                case RESTMethod.DELETE:
                    req = UnityWebRequest.Delete(uri);
                    break;
                default:
                    return null;
            }

            req.method = ToMethodString(method);

            return req;
        }

        private string ToMethodString(RESTMethod method)
        {
            switch (method)
            {
                case RESTMethod.GET:
                    return "GET";
                case RESTMethod.POST:
                    return "POST";
                case RESTMethod.PATCH:
                    return "PATCH";
                case RESTMethod.DELETE:
                    return "DELETE";
                default:
                    return string.Empty;
            }
        }
    }

    public abstract class SexyRESTWorkParent : IRESTSubject
    {
        protected Action<IRESTResponse> callback = null;

        public virtual IRESTSubject Subscribe(Action<IRESTResponse> callback)
        {
            this.callback = callback;

            return this;
        }

        protected void SetTimeout(UnityWebRequest target, int timeout)
        {
            if (timeout > 0)
                target.timeout = timeout;
        }

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

        protected RESTResponse MakeRESTResponse(UnityWebRequest target, bool includeResponseHeaders)
        {
            if (includeResponseHeaders)
                return new RESTResponse(target.responseCode, target.downloadHandler.text, target.error, ToRESTResult(target.result), target.GetResponseHeaders());
            else
                return new RESTResponse(target.responseCode, target.downloadHandler.text, target.error, ToRESTResult(target.result));
        }

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
    }

    public class SexyRESTWorker : SexyRESTWorkParent
    {
        public SexyRESTWorker Request(UnityRESTReceipt receipt, bool includeResponseHeaders = false)
        {
            MonoHelper.StartCoroutine(CoRequest(receipt, includeResponseHeaders));

            return this;
        }

        private IEnumerator CoRequest(UnityRESTReceipt receipt, bool includeResponseHeaders)
        {
            using (UnityWebRequest req = receipt.GetUnityWebRequest())
            {
                // 타임아웃 설정
                SetTimeout(req, receipt.timeout);
                // 헤더 설정
                SetRequestHeaders(req, receipt.headers);

                // 요청 전달
                yield return req.SendWebRequest();

                RESTResponse res = MakeRESTResponse(req, includeResponseHeaders);

                callback?.Invoke(res);
            }
        }
    }

    public class SexyPostableRESTWorker : SexyRESTWorkParent
    {
        public SexyPostableRESTWorker Request(UnityPostableRESTReceipt receipt, bool includeResponseHeaders = false)
        {
            MonoHelper.StartCoroutine(CoRequest(receipt, includeResponseHeaders));

            return this;
        }

        private IEnumerator CoRequest(UnityPostableRESTReceipt receipt, bool includeResponseHeaders)
        {
            using (UnityWebRequest req = receipt.GetUnityWebRequest())
            {
                /// 아래 내용은 receipt.GetUnityWebRequest함수 내부(UnityWebRequest)에서 수행되기 때문에 일단 동작시키지 않는다
#if false
                // post data를 UTF8 byte 형식으로 변경하여 업로드
                if (!string.IsNullOrEmpty(receipt.body))
                {
                    byte[] bytes = new UTF8Encoding().GetBytes(receipt.body);
                    req.uploadHandler = new UploadHandlerRaw(bytes);
                }
#endif

                // 타임아웃 설정
                SetTimeout(req, receipt.timeout);
                // 헤더 설정
                SetRequestHeaders(req, receipt.headers);

                // 요청 전달
                yield return req.SendWebRequest();

                RESTResponse res = MakeRESTResponse(req, includeResponseHeaders);

                callback?.Invoke(res);
            }
        }
    }
}