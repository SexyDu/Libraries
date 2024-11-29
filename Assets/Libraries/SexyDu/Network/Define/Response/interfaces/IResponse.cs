using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// Network 수신 데이터 인터페이스
    /// </summary>
    public interface IResponse
    {
        // 수신 코드
        public long code { get; }
        // 수신 에러
        public string error { get; }
        // 수신 결과
        public NetworkResult result { get; }
        // 수신 헤더 (딕셔너리)
        public Dictionary<string, string> headers { get; }

        /// <summary>
        /// REST API 성공 여부
        /// </summary>
        public bool IsSuccess => result == NetworkResult.Success || result == NetworkResult.SuccessFromCache;
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