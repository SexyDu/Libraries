using System;
using System.Collections.Generic;

namespace SexyDu.Network
{
    /// ### 여기서 struct를 사용한 이유
    ///     단순 데이터 처리이기 때문에 class를 사용하지 않고 struct를 사용함

    /// <summary>
    /// REST API 요청 접수증
    /// </summary>
    public struct RESTReceipt : IRESTReceipt
    {
        public Uri uri
        {
            get;
            private set;
        }

        public NetworkMethod method
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

        public RESTReceipt(NetworkMethod method)
        {
            this.method = method;

            uri = null;
            timeout = 0;
            headers = null;
            body = string.Empty;
        }

        #region Builder
        public RESTReceipt SetUri(string url)
        {
            this.uri = new Uri(url);
            return this;
        }
        public RESTReceipt SetMethod(NetworkMethod method)
        {
            this.method = method;
            return this;
        }
        public RESTReceipt SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }
        public RESTReceipt SetHeaders(Dictionary<string, string> headers)
        {
            this.headers = headers;
            return this;
        }
        public RESTReceipt AddHeader(string key, string value)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();
            headers.Add(key, value);

            return this;
        }
        public RESTReceipt SetBody(string body)
        {
            this.body = body;
            return this;
        }
        #endregion
    }
}