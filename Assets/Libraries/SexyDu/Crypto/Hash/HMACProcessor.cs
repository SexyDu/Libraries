using System;
using System.Security.Cryptography;

namespace SexyDu.Crypto
{
    /// <summary>
    /// HMAC을 사용하여 byte array 암호화 검증 처리
    /// </summary>
    public class HMACProcessor : IDisposable
    {
        // HMAC 암호기
        private readonly HMACSHA256 computer = null;
        // HMAC 길이
        /// HMACSHA256은 256bit(32byte)의 해시값을 반환하기에 상수로 정의
        private const int HMAC_LENGTH = 32;

        public HMACProcessor(char[] base64key) : this(Convert.FromBase64CharArray(base64key, 0, base64key.Length)) { }

        public HMACProcessor(byte[] key)
        {
            computer = new HMACSHA256(key);
        }

        public void Dispose()
        {
            computer.Dispose();
        }

        /// <summary>
        /// HMAC 검증 데이터 부착 (Postfix)
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>HMAC 검증 데이터</returns>
        public byte[] Attach(byte[] data)
        {
            return BufferTool.Combine(data, computer.ComputeHash(data));
        }

        /// <summary>
        /// HMAC 검증 데이터 분할 (Skim)
        /// </summary>
        /// <param name="whole">전체 데이터 (HMAC 검증 데이터 포함)</param>
        /// <returns>실제(암호화된) 데이터</returns>
        public byte[] Skim(byte[] whole)
        {
            if (whole == null)
            {
                throw new ArgumentNullException("Skim 처리될 데이터가 없습니다.");
            }
            if (whole.Length <= computer.Key.Length)
            {
                throw new HmacVerificationException("HMAC 검증 실패: HMAC 검증 데이터가 부족합니다.");
            }
            else
            {
                // data(encrypted) 영역과 hmac 영역 분할
                (byte[] data, byte[] hmacHash) = BufferTool.Split(whole, whole.Length - HMAC_LENGTH);
                if (!BufferTool.Compare(hmacHash, computer.ComputeHash(data)))
                    throw new HmacVerificationException("HMAC 검증 실패: 데이터가 변조되었을 수 있습니다.");
                return data;
            }

        }
        /// <summary>
        /// HMAC 검증
        /// </summary>
        /// <param name="whole">전체 데이터 (HMAC 검증 데이터 포함)</param>
        /// <returns>검증 성공 여부</returns>
        public bool Verify(byte[] whole)
        {
            // data(encrypted 영역과 hmac 영역 분할
            (byte[] data, byte[] hmacHash) = BufferTool.Split(whole, whole.Length - HMAC_LENGTH);

            return BufferTool.Compare(hmacHash, computer.ComputeHash(data));
        }
    }
}