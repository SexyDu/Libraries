using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SexyDu.Crypto
{
    /// <summary>
    /// byte array AES 암호화 클래스
    /// </summary>
    public class AesBytes : AesEncryptor, IBytesEncryptor
    {
        public AesBytes() : base() { }

        public AesBytes(byte[] key, byte[] iv) : base(key, iv) { }

        public AesBytes(char[] key, char[] iv) : base(key, iv) { }

        public override void Dispose()
        {
            base.Dispose();

            if (UseHmac)
            {
                hmac.Dispose();
                hmac = null;
            }
        }

        /// <summary>
        /// 데이터 암호화
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>암호화 데이터</returns>
        public byte[] Encrypt(byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var encryptor = aes.CreateEncryptor())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock(); // 암호화 스트림 마지막 블록 처리

                if (UseHmac)
                    return hmac.Attach(ms.ToArray());
                else
                    return ms.ToArray();
            }
        }

        /// <summary>
        /// 비동기 데이터 암호화
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>암호화 데이터</returns>
        public async Task<byte[]> EncryptAsync(byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var encryptor = aes.CreateEncryptor())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                await cs.WriteAsync(data, 0, data.Length);
                cs.FlushFinalBlock(); // 암호화 스트림 마지막 블록 처리
                
                if (UseHmac)
                    return hmac.Attach(ms.ToArray());
                else
                    return ms.ToArray();
            }
        }

        /// <summary>
        /// 데이터 복호화
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>복호화 데이터</returns>
        public byte[] Decrypt(byte[] data)
        {
            if (UseHmac)
                data = hmac.Skim(data);

            using (var ms = new MemoryStream())
            using (var decryptor = aes.CreateDecryptor())
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock(); // 암호화 스트림 마지막 블록 처리
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 비동기 데이터 복호화
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>복호화 데이터</returns>
        public async Task<byte[]> DecryptAsync(byte[] data)
        {
            if (UseHmac)
                data = hmac.Skim(data);

            using (var ms = new MemoryStream())
            using (var decryptor = aes.CreateDecryptor())
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                await cs.WriteAsync(data, 0, data.Length);
                cs.FlushFinalBlock(); // 암호화 스트림 마지막 블록 처리
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 데이터 무결성 인증
        ///  * 데이터 무결성 인증을 사용하려면 SetHmac함수를 통해 HMAC 객체를 생성한다.
        /// </summary>
        #region HMAC
        // HMAC 객체
        private HMACProcessor hmac = null;
        // HMAC 사용 여부
        public bool UseHmac => hmac != null;
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="base64key">base64 문자열 HMAC 키</param>
        public IBytesEncryptor SetHmac(char[] base64key)
        {
            return SetHmac(Convert.FromBase64CharArray(base64key, 0, base64key.Length));
        }
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="key">HMAC 키</param>
        public IBytesEncryptor SetHmac(byte[] key)
        {
            hmac = new HMACProcessor(key);
            return this;
        }
        #endregion
    }
}