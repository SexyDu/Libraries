using System;
using System.IO;
using System.Security.Cryptography;

namespace SexyDu.Crypto
{
    /// <summary>
    /// byte array AES 암호화 클래스
    /// </summary>
    public class AesBytes : AesEncryptor, IEncryptBytes, IDecryptBytes
    {
        public AesBytes() : base() { }

        public AesBytes(byte[] key, byte[] iv) : base(key, iv) { }

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
                    return ApplyHmac(ms.ToArray());
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
                data = SkimHmac(data);

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
        /// 데이터 무결성 인증
        ///  * 데이터 무결성 인증을 사용하려면 SetHmac함수를 통해 HMAC 객체를 생성한다.
        /// </summary>
        #region HMAC
        // HMAC 객체
        private HMACSHA256 hmac = null;
        // HMAC 사용 여부
        private bool UseHmac => hmac != null;
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="base64key">base64 문자열 HMAC 키</param>
        public AesBytes SetHmac(char[] base64key)
        {
            return SetHmac(Convert.FromBase64CharArray(base64key, 0, base64key.Length));
        }
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="key">HMAC 키</param>
        public AesBytes SetHmac(byte[] key)
        {
            hmac = new HMACSHA256(key);
            return this;
        }
        /// <summary>
        /// HMAC 해시 계산
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>HMAC 해시</returns>
        private byte[] GetHmacHash(byte[] data)
        {
            return hmac.ComputeHash(data);
        }
        /// <summary>
        /// HMAC 적용 데이터 반환
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>HMAC 적용 데이터</returns>
        private byte[] ApplyHmac(byte[] data)
        {
            return BufferTool.Combine(data, GetHmacHash(data));
        }
        /// <summary>
        /// HMAC 제거 데이터 반환
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>HMAC 제거 데이터</returns>
        private byte[] SkimHmac(byte[] data)
        {
            // encrypt 영역과 hmac 영역 분할
            (byte[] encrypted, byte[] hmacHash) = BufferTool.Split(data, data.Length - hmac.Key.Length);

            byte[] computedHash = hmac.ComputeHash(encrypted);
            if (!BufferTool.Compare(computedHash, hmacHash))
                throw new HmacVerificationException("HMAC 검증 실패: 데이터가 변조되었을 수 있습니다.");
            return encrypted;
        }

        
        #endregion

#if UNITY_EDITOR
        private string ToString(byte[] bytes)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(bytes[0].ToString());
            for (int i = 1; i < bytes.Length; i++)
                sb.AppendFormat(", {0}", bytes[i]);
            return sb.ToString();
        }
#endif
    }
}