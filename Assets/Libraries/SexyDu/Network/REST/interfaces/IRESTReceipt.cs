using System;
using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// REST API 요청 접수증 기본 인터페이스
    /// </summary>
    public interface IRESTReceipt
    {
        // 요청 Uri
        public Uri uri { get; }
        // 요청 형식, GET/POST/PATCH/DELETE
        public NetworkMethod method { get; }
        // 요청 타임아웃
        public int timeout { get; }
        // 요청 헤더
        public Dictionary<string, string> headers { get; }
        // 요청 포스트 데이터
        public string body { get; }
    }
}