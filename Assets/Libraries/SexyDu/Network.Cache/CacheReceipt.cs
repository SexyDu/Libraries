using System;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// Binary 요청 접수증
    /// </summary>
    public struct CacheReceipt : ICacheReceipt
    {
        public Uri uri
        {
            get;
            private set;
        }

        public int timeout
        {
            get;
            private set;
        }

        public ICacheEncryptor encryptor
        {
            get;
            private set;
        }

        #region Builder
        public CacheReceipt SetUri(string url)
        {
            this.uri = new Uri(url);
            return this;
        }
        public CacheReceipt SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }
        public CacheReceipt SetEncryptor(ICacheEncryptor encryptor)
        {
            this.encryptor = encryptor;
            return this;
        }
        #endregion
    }
}