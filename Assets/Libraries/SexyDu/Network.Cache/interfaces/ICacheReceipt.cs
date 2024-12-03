namespace SexyDu.Network.Cache
{
    public interface ICacheReceipt : IBinaryReceipt
    {
        // 암호화 인터페이스
        public ICacheEncryptor encryptor { get; }

        /// <summary>
        /// 캐시 파일명 반환
        /// </summary>
        /// <returns>캐시 파일명</returns>
        public string GetCacheFileName();
    }
}