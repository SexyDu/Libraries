using System;
using System.Collections.Generic;

namespace SexyDu.Network.Cache
{
    . // 주석 달고 코드 정리
    public partial class CacheCloud : ICacheCloud//(IRequestableCacheEntry, IRemovableCacheEntry)
    {
        private readonly Dictionary<string, CacheEntry> entries = new();

        public bool HasEntry<T>(string url) => HasEntry(GetKey<T>(url));
        private bool HasEntry(string key) => entries.ContainsKey(key);

        public ICacheEntry GetEntry<T>(string url)
        {
            string key = GetKey<T>(url);
            if (HasEntry(key))
                return entries[key];
            else
                return null;
        }

        private string GetKey<T>(string url)
        {
            return GetKey(typeof(T), url);
        }

        private string GetKey(Type type, string url)
        {
            return string.Format("{0}:{1}", type.Name, url);
        }

        public ICacheEntry Request<T>(ICacheReceipt receipt)
        {
            Type requestedType = typeof(T);
            string key = GetKey(requestedType, receipt.uri.AbsoluteUri);

            if (entries.ContainsKey(key))
                return entries[key];
            else
                return Request(requestedType, key, receipt);
        }

        public void Remove<T>(string url) => Remove(GetKey<T>(url));

        public void Remove(Type type, string url) => Remove(GetKey(type, url));

        private void Remove(string key)
        {
            if (entries.ContainsKey(key))
            {
                if (!entries[key].IsDisposed)
                    entries[key].Dispose();
                entries.Remove(key);
            }
        }
    }

    public class CacheEntry : ICacheEntry
    {
        public readonly string url = null;
        public readonly Type type = null;
        public object data
        {
            get;
            private set;
        }

        public IRemovableCacheEntry manager = null;

        public string Url => url;
        public Type Type => type;

        public CacheEntry(string url, Type type)
        {
            this.url = url;
            this.type = type;
        }

        // baskets이 null이면 dispose 된 것으로 간주
        public bool IsDisposed => baskets == null;

        public void Dispose()
        {
            if (IsDisposed)
                return;

            if (baskets != null)
            {
                foreach (var basket in baskets)
                    basket.OnBrokenEntry();
                baskets.Clear();
                baskets = null;
            }

            if (data != null)
            {
                DestroyIfUnityObject(data);
                data = null;
            }

            manager.Remove(type, url);
            manager = null;
        }

        public CacheEntry Set(IRemovableCacheEntry manager)
        {
            this.manager = manager;
            return this;
        }

        public CacheEntry Set(object data)
        {
            if (!IsDisposed)
            {
                this.data = data;

                // basket이 있는 경우 배포
                if (baskets.Count > 0)
                    Distribute();
                // basket이 없는 경우 Dispose
                else
                    Dispose();
            }
            else
                DestroyIfUnityObject(data);

            return this;
        }

        private List<ICacheBasket> baskets = new();

        public ICacheEntry AddBasket(ICacheBasket basket)
        {
            if (IsDisposed)
                return null;

            baskets.Add(basket);

            if (data != null)
                basket.Pour(data);

            return this;
        }

        public ICacheEntry RemoveBasket(ICacheBasket basket)
        {
            if (IsDisposed)
                return null;

            baskets.Remove(basket);

            if (baskets.Count == 0)
                Dispose();

            return this;
        }

        private void Distribute()
        {
            foreach (var basket in baskets)
                basket.Pour(data);
        }

        private void DestroyIfUnityObject(object obj)
        {
            if (obj is UnityEngine.Object unityObject)
                UnityEngine.Object.Destroy(unityObject);
        }
    }
}