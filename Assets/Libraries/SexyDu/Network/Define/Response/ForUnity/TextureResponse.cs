using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Network
{
    /// <summary>
    /// 유니티 텍스쳐(2D) 수신 데이터
    /// </summary>
    public struct TextureResponse : ITextureResponse
    {
        // 수신받은 데이터
        public readonly Texture2D tex
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

        public TextureResponse(Texture2D tex2D, long code, string error, NetworkResult result, Dictionary<string, string> headers = null)
        {
            this.tex = tex2D;
            this.code = code;
            this.error = error;
            this.result = result;
            this.headers = headers;
        }

        /// <summary>
        /// 빈 수신 데이터
        /// </summary>
        public static readonly TextureResponse Empty = new TextureResponse(null, long.MinValue, string.Empty, NetworkResult.ConnectionError);
    }
}