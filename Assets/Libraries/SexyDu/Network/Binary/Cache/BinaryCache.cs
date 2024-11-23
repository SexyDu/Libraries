namespace SexyDu.Network
{
    public abstract class BinaryCache : ICache
    {
        /// <summary>
        /// 다운로더 반환
        /// </summary>
        /// <returns>다운로더</returns>
        protected IBytesDownloader GetDownloader()
        {
            return new SexyBytesDownloader();
        }


        // . // 이제부터 캐시 진행하자. 일단 기본적인거 하고 텍스쳐까지만
    }
}