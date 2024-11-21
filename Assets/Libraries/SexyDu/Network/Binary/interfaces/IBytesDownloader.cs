using System;

namespace SexyDu.Network
{
    /// <summary>
    /// Byte array 다운로드 작업자 인터페이스
    /// </summary>
    public interface IBytesDownloader : IBytesSubject
    {
        public IBytesDownloader Request(IBinaryReceipt receipt);
    }
    /// <summary>
    /// Byte array 다운로드 콜백 옵저버 서브젝트 인터페이스
    /// </summary>
    public interface IBytesSubject
    {
        public IBytesSubject Subscribe(Action<IBytesResponse> callback);
    }
}