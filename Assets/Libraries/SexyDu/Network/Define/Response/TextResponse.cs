using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// 텍스트 수신 데이터
    /// </summary>
    public struct TextResponse : ITextResponse
    {
        // response code
        public readonly long code
        {
            get;
        }
        // 수신받은 데이터
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
        public readonly NetworkResult result
        {
            get;
        }
        // 헤더 정보
        public readonly Dictionary<string, string> headers
        {
            get;
        }

        public TextResponse(string text, long code, string error, NetworkResult result, Dictionary<string, string> headers = null)
        {
            this.text = text;
            this.code = code;
            this.error = error;
            this.result = result;
            this.headers = headers;
        }

        /// <summary>
        /// 빈 수신 데이터
        /// </summary>
        public static readonly TextResponse Empty = new TextResponse(string.Empty, long.MinValue, string.Empty, NetworkResult.ConnectionError);
    }
}