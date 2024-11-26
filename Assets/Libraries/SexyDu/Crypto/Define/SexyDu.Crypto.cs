using System;

namespace SexyDu
{
    namespace Crypto
    {
        #region Exceptions
        public class HmacVerificationException : Exception
        {
            public HmacVerificationException(string message) : base(message) { }
        }
        #endregion
    }
}
