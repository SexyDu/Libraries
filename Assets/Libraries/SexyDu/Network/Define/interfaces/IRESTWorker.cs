using System;

namespace SexyDu.Network
{
    /// <summary>
    /// REST API 작업자 인터페이스
    /// </summary>
    public interface IRESTWorker
    {
        /// <summary>
        /// REST API 수신 시 Response 콜백 등록
        /// </summary>
        /// <param name="callback">콜백 함수</param>
        /// <returns>자기 자신 인터페이스</returns>
        public IRESTWorker Subscribe(Action<IRESTResponse> callback);
    }
}