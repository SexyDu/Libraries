// #define FIX_FILE_NAME

using System;
using SexyDu.Crypto;

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

        #region FileName
        /// <summary>
        /// 캐시 파일명 반환
        /// </summary>
        /// <returns>캐시 파일명</returns>
        public string GetCacheFileName()
        {
            using (SHA256Encryptor hash = new SHA256Encryptor())
            {
#if FIX_FILE_NAME
                return hash.Encrypt(uri.AbsoluteUri, GetBaseHashSalt());
#else
                // 암호화가 없는 경우  BaseHashSalt 사용
                if (encryptor == null)
                    return hash.Encrypt(uri.AbsoluteUri, GetBaseHashSalt());
                // 암호화가 있는 경우
                else
                {
                    // HMAC 사용 시 HMAC Key 및 IV 사용
                    if (encryptor.UseHmac)
                        return hash.Encrypt(uri.AbsoluteUri, encryptor.HmacKey, encryptor.Iv);
                    // HMAC 미사용 시 BaseHashSalt 및 IV 사용
                    else
                        return hash.Encrypt(uri.AbsoluteUri, GetBaseHashSalt(), encryptor.Iv);
                }
#endif
            }
        }

        /// <summary>
        /// 파일명 암호화에 사용될 기본 Salt
        /// </summary>
        private char[] GetBaseHashSalt()
        {
            /// string : RvLMAT5gi+kmM4DnzJs5nA==
            /// EncryptionKeyGenerator (EditorWindow 'SexyDu/EncryptionKeyGenerator')에서 생성 <summary>
            return new char[24] { 'R', 'v', 'L', 'M', 'A', 'T', '5', 'g', 'i', '+', 'k', 'm', 'M', '4', 'D', 'n', 'z', 'J', 's', '5', 'n', 'A', '=', '=' };
        }
        #endregion
    }
}