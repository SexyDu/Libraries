using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// REST 수신 데이터 인터페이스
    /// </summary>
    public interface IRESTResponse
    {
        // 수신 코드
        public long code { get; }
        // 수신 문자열
        public string text { get; }
        // 수신 에러
        public string error { get; }
        // 수신 결과
        public RESTResult result { get; }
        // 수신 헤더 (딕셔너리)
        public Dictionary<string, string> headers { get; }

        /// <summary>
        /// REST API 성공 여부
        /// </summary>
        public bool IsSuccess => result == RESTResult.Success;
        /// <summary>
        /// Empty 여부
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty => code == long.MinValue;
        /// <summary>
        /// InternalServerError 여부
        /// </summary>
        public bool IsInternalServerError => code == 500;
    }
}