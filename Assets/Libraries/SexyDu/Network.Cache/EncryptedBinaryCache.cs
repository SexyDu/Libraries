using SexyDu.Crypto;
using SexyDu.FileIO;

namespace SexyDu.Network.Cache
{
    public abstract class EncryptedBinaryCache : BinaryCache
    {
        public EncryptedBinaryCache()
        {
        }

        /// <summary>
        /// 암호화 키 및 초기화 벡터 설정
        /// </summary>
        /// <param name="key">암호화 키(base64 44 length = 32byte = 256bit)</param>
        /// <param name="iv">초기화 벡터(base64 24 length = 16byte = 128bit)</param>
        public EncryptedBinaryCache(char[] key, char[] iv)
        {
            encryptionKey = key;
            encryptionIv = iv;
        }

        public override void Dispose()
        {
            base.Dispose();

            hmacKey = null;
        }

        /// <summary>
        /// 기본 암호화 검증기 설정
        /// </summary>
        public EncryptedBinaryCache SetHmac()
        {
            hmacKey = GetDefaultHmacKey();
            return this;
        }
        /// <summary>
        /// 암호화 검증기 설정
        /// </summary>
        /// <param name="hmacKey">검증기 키(base64 44 length = 32byte = 256bit)</param>
        public EncryptedBinaryCache SetHmac(char[] hmacKey)
        {
            this.hmacKey = hmacKey;
            return this;
        }

        #region Load
        /// <summary>
        /// 파일 리더 반환
        /// </summary>
        /// <returns>파일 리더</returns>
        protected override IFileAsyncReader MakeFileReader()
        {
            var reader = new AesFileAsyncHandler(encryptionKey, encryptionIv);
            if (UseHmac)
                reader.SetHmac(hmacKey);
            return reader;
        }
        /// <summary>
        /// 파일 라이터 반환
        /// </summary>
        /// <returns>파일 라이터</returns>
        protected override IFileAsyncWriter MakeFileWriter()
        {
            var writer = new AesFileAsyncHandler(encryptionKey, encryptionIv);
            if (UseHmac)
                writer.SetHmac(hmacKey);
            return writer;
        }
        #endregion

        #region File
        /// <summary>
        /// Url에 따른 파일명 반환
        /// * 암호화 캐시의 경우 파일명을 다르게 하기 위함
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>파일명</returns>
        protected override string GetCacheName(string url)
        {
            using (SHA256Encryptor encryptor = new SHA256Encryptor())
            {
                // 검증기 사용 시 검증기와 iv를 사용하여 파일명 안호화
                if (UseHmac)
                    return encryptor.Encrypt(url, hmacKey, encryptionIv);
                // 검증기 미사용 시 기본 해쉬 솔트와 iv를 사용하여 파일명 안호화
                else
                    return encryptor.Encrypt(url, BaseHashSalt, encryptionIv);
            }
        }
        #endregion

        #region Default Encryption Value
        // 암호화 Key
        /// default : OJwI+0BGHADV4IQj8Nmzq7mdqbnaU/q8vh/S/ussZGU=
        private readonly char[] encryptionKey = new char[44] { 'O', 'J', 'w', 'I', '+', '0', 'B', 'G', 'H', 'A', 'D', 'V', '4', 'I', 'Q', 'j', '8', 'N', 'm', 'z', 'q', '7', 'm',
             'd', 'q', 'b', 'n', 'a', 'U', '/', 'q', '8', 'v', 'h', '/', 'S', '/', 'u', 's', 's', 'Z', 'G', 'U', '=' };
        // 암호화 IV
        /// default : ioxyC/yzlwSBXs5uMtA==
        private readonly char[] encryptionIv = new char[24] { 'i', 'o', 'x', 'y', 'C', '/', 'y', 'z', 'l', 'w', 's', 'B', 'X', 's', '5', 'u', 'M', 't', 'a', '7', '4', 'A', '=', '=' };
        // 검증기 HMAC Key
        private char[] hmacKey = null;
        // 검증기 사용 여부
        private bool UseHmac => hmacKey != null;
        /// <summary>
        /// 기본 HMAC Key 반환
        /// </summary>
        /// <returns>HMAC Key</returns>
        private char[] GetDefaultHmacKey()
        {
            // fYxJogT0FhHB/BEZYFMBfhqGiAIn90oaDvsxBdWPzO4=
            return new char[44] { 'f', 'Y', 'x', 'J', 'o', 'g', 'T', '0', 'F', 'h', 'H', 'B', '/', 'B', 'E', 'Z', 'Y', 'F', 'M', 'B', 'f', 'h', 'q', 'G', 'i', 'A', 'I', 'n', '9',
                '0', 'o', 'a', 'D', 'v', 's', 'x', 'B', 'd', 'W', 'P', 'z', 'O', '4', '=' };
        }
        #endregion
    }
}