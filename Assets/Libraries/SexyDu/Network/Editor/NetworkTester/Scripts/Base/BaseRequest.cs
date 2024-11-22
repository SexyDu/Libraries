using System;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 요청 베이스
    /// </summary>
    [Serializable]
    public abstract class BaseRequest : IClearable
    {
        // 요청 url
        public string url = string.Empty;
        // 네트워크 method
        public NetworkMethod method = NetworkMethod.GET;
        // 타임아웃
        public int timeout = 0;

        /// <summary>
        /// 클리어
        /// </summary>
        public virtual void Clear()
        {
            url = string.Empty;
            method = NetworkMethod.GET;
            timeout = 0;
        }
    }
}

