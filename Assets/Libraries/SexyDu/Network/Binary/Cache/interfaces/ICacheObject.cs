namespace SexyDu.Network
{
    public interface ICache : INetworker
    {
        // .
    }

    /// <summary>
    /// 관리형 캐시
    /// </summary>
    public interface IManagedCache : ICache
    {
        /// <summary>
        /// 캐시 url
        /// </summary>
        public string Url { get; }
    }
}