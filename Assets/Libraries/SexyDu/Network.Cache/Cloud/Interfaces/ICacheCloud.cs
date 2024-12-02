using System;

namespace SexyDu.Network.Cache
{
    public interface ICacheCloud : IRequestableCacheEntry, IRemovableCacheEntry
    {
        public bool HasEntry<T>(string url);

        public ICacheEntry GetEntry<T>(string url);
    }

    public interface IRequestableCacheEntry
    {
        public ICacheEntry Request<T>(ICacheReceipt receipt);
    }

    public interface IRemovableCacheEntry
    {
        public void Remove<T>(string url);
        public void Remove(Type type, string url);
    }

    public interface ICacheEntry : IDisposable
    {
        public string Url { get; }
        public Type Type { get; }

        public ICacheEntry AddBasket(ICacheBasket basket);
        public ICacheEntry RemoveBasket(ICacheBasket basket);
    }
}