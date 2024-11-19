using System;
using System.Collections.Generic;

namespace SexyDu.Network
{
    /// ### 여기서 struct를 사용한 이유
    ///     단순 데이터 처리이기 때문에 class를 사용하지 않고 struct를 사용함

    /// <summary>
    /// UnityWebRequest를 사용한 REST API 접수증
    /// </summary>
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

        #region Builder
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
        #endregion
    }

    /// <summary>
    /// Post(body)할 데이터가 포함된 UnityWebRequest를 사용한 REST API 접수증
    /// </summary>
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

        #region Builder
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
        #endregion
    }
}