namespace SexyDu.Network.Cache
{
    public interface ICacheReceipt : IBinaryReceipt
    {
        // 암호화 인터페이스
        public ICacheEncryptor encryptor { get; }
    }
}