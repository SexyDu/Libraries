using System;

namespace SexyDu.Network
{
    /// <summary>
    /// Byte array 다운로드 작업자 인터페이스
    /// </summary>
    public interface IBytesDownloader : IBytesSubject
    {
        /// <summary>
        /// 접수증을 받아 다운로드 작업을 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>작업자</returns>
        public IBytesDownloader Request(IBinaryReceipt receipt);
    }
    /// <summary>
    /// Byte array 다운로드 콜백 옵저버 서브젝트 인터페이스
    /// </summary>
    public interface IBytesSubject
    {
        /// <summary>
        /// 수신 콜백 등록
        /// </summary>
        public IBytesSubject Subscribe(Action<IBytesResponse> callback);
    }
}