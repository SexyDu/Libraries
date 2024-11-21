using System;

namespace SexyDu.Network
{
    /// <summary>
    /// Binary 요청 접수증
    /// </summary>
    public class BinaryReceipt : IBinaryReceipt
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

        #region Builder
        public BinaryReceipt SetUri(string url)
        {
            this.uri = new Uri(url);
            return this;
        }
        public BinaryReceipt SetTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }
        #endregion
    }
}