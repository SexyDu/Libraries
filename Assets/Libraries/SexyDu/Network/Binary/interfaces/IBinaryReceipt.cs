using System;

namespace SexyDu.Network
{
    /// <summary>
    /// Binary 요청 접수증 기본 인터페이스
    /// </summary>
    public interface IBinaryReceipt
    {
        // 요청 Uri
        public Uri uri { get; }
        // 요청 타임아웃
        public int timeout { get; }
        // 요청 헤더
        // public Dictionary<string, string> headers { get; }
    }
}