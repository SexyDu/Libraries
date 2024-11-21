using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// Byte Array 수신 데이터
    /// </summary>
    public struct BytesResponse : IBytesResponse
    {
        // response code
        public readonly long code
        {
            get;
        }
        // 수신받은 데이터
        public readonly byte[] data
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

        public BytesResponse(byte[] data, long code, string error, NetworkResult result, Dictionary<string, string> headers = null)
        {
            this.data = data;
            this.code = code;
            this.error = error;
            this.result = result;
            this.headers = headers;
        }

        /// <summary>
        /// 빈 수신 데이터
        /// </summary>
        public static readonly BytesResponse Empty = new BytesResponse(null, long.MinValue, string.Empty, NetworkResult.ConnectionError);
    }
}