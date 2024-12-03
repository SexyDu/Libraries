using System;
using System.Collections.Generic;

namespace SexyDu.Network
{
    /// <summary>
    /// 네티워크 수신 데이터
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Response<T> : IResponse<T> where T : class
    {
        // 수신 데이터
        public readonly T content
        {
            get;
        }
        // response code
        public readonly long code
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

        public Response(T content, long code, string error, NetworkResult result, Dictionary<string, string> headers = null)
        {
            this.content = content;
            this.code = code;
            this.error = error;
            this.result = result;
            this.headers = headers;
        }

        public Response(T data, IResponse response) : this(data, response.code, response.error, response.result, response.headers) { }

        /// <summary>
        /// 수신 데이터(리소스) 릴리즈
        /// </summary>
        public void Release()
        {
            if (content is IReleasable releasable)
                releasable.Release();
            else if (content is UnityEngine.Object unityObject)
                UnityEngine.Object.Destroy(unityObject);
        }

        /// <summary>
        /// 빈 수신 데이터
        /// </summary>
        public static readonly Response<T> Empty = new Response<T>(null, long.MinValue, string.Empty, NetworkResult.Unknown);
    }
}