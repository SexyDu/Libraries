using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
        /// REST 수신 데이터
        /// </summary>
        public struct RESTResponse : IRESTResponse
        {
            // response code
            public readonly long code
            {
                get;
            }
            // 수신받은 Text 문자열 (json 등)
            public readonly string text
            {
                get;
            }
            // 에러 문자열
            public readonly string error
            {
                get;
            }
            // 결과
            public readonly RESTResult result
            {
                get;
            }
            // 헤더 정보
            public readonly Dictionary<string, string> headers
            {
                get;
            }

            public RESTResponse(long code, string text, string error, RESTResult result, Dictionary<string, string> headers = null)
            {
                this.code = code;
                this.text = text;
                this.error = error;
                this.result = result;
                this.headers = headers;
            }

            /// <summary>
            /// 빈 수신 데이터
            /// </summary>
            public static readonly RESTResponse Empty = new RESTResponse(long.MinValue, string.Empty, string.Empty, RESTResult.ConnectionError);
        }
}