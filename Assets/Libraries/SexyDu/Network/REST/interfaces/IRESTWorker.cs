using System;

namespace SexyDu.Network
{
    /// <summary>
    /// REST API 작업자 인터페이스
    /// </summary>
    public interface IRESTWorker : IRESTSubject, INetworker
    {
        /// <summary>
        /// REST API 요청
        /// </summary>
        public IRESTWorker Request(IRESTReceipt receipt);
    }

    /// <summary>
    /// Post 데이터를 포함한 REST API 작업자 인터페이스
    /// </summary>
    public interface IPostableRESTWorker : IRESTSubject
    {
        /// <summary>
        /// REST API 요청
        /// </summary>
        public IPostableRESTWorker Request(IPostableRESTReceipt receipt);
    }

    /// <summary>
    /// REST API 콜백 옵저버 서브젝트 인터페이스
    /// </summary>
    public interface IRESTSubject :  IDisposable
    {
        /// <summary>
        /// REST API 수신 시 Response 콜백 등록
        /// </summary>
        /// <param name="callback">콜백 함수</param>
        /// <returns>자기 자신 인터페이스</returns>
        public IRESTSubject Subscribe(Action<ITextResponse> callback);
    }
}