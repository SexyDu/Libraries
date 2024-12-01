using System;

namespace SexyDu.Network.Cache
{
    public interface ICacheCloud : IRequestableCacheEntry, IRemovableCacheEntry { }

    public interface IRequestableCacheEntry
    {
        public ICacheEntry Request<T>(IBinaryReceipt receipt);
    }

    public interface IRemovableCacheEntry
    {
        public void Remove(string url);
    }

    public interface ICacheEntry : IDisposable
    {
        public string Url { get; }
        public Type Type { get; }

        public void AddBasket(ICacheBasket basket);
        public void RemoveBasket(ICacheBasket basket);
    }
}