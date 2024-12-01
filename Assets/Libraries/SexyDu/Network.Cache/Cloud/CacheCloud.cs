using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    // . // CacheCloud 다시 테스트해보자
    public class CacheCloud : ICacheCloud//(IRequestableCacheEntry, IRemovableCacheEntry)
    {
        private readonly Dictionary<string, CacheEntry> entries = new();

        public ICacheEntry Request<T>(IBinaryReceipt receipt)
        {
            if (entries.ContainsKey(receipt.uri.AbsoluteUri))
            {
                if (entries[receipt.uri.AbsoluteUri].Type == typeof(T))
                    return entries[receipt.uri.AbsoluteUri];
                else
                    throw new TypeAccessException($"Type mismatch: {entries[receipt.uri.AbsoluteUri].Type.Name}(Loaded) != {typeof(T).Name}(Requested)");
            }
            else
            {
                if (typeof(T) == typeof(Texture2D))
                    return RequestTexture(receipt);
                else
                    throw new NotSupportedException($"Not supported type: {typeof(T).Name}");
            }
        }

        private ICacheEntry RequestTexture(IBinaryReceipt receipt)
        {
            if (!entries.ContainsKey(receipt.uri.AbsoluteUri))
            {
                CacheEntry entry = new CacheEntry(receipt.uri.AbsoluteUri, typeof(Texture2D));
                entries[entry.url] = entry;

                new TextureCache().Request(receipt).Subscribe(res =>
                {
                    if (res.IsSuccess)
                    {
                        if (!entry.IsDisposed)
                            entry.Set(res.tex);
                        else
                            UnityEngine.Object.Destroy(res.tex);
                    }
                    else
                    {
                        /// TODO: 실패일 경우 원인 파악하고 Dictionary에서 삭제 코드 넣자
                        entry.Dispose();
                    }
                });
            }

            return entries[receipt.uri.AbsoluteUri];
        }

        public void Remove(string url)
        {
            if (entries.ContainsKey(url))
                entries[url].Dispose();
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

        // url이 null이면 dispose 된 것으로 간주
        public bool IsDisposed => manager == null;

        public void Dispose()
        {
            foreach (var basket in baskets)
                basket.OnBrokenEntry();
            baskets.Clear();

            if (data != null)
            {
                DestroyIfUnityObject(data);
                data = null;
            }

            manager.Remove(url);
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

        private readonly List<ICacheBasket> baskets = new();

        public void AddBasket(ICacheBasket basket)
        {
            if (IsDisposed)
                return;

            baskets.Add(basket);

            if (data != null)
                basket.Pour(data);
        }

        public void RemoveBasket(ICacheBasket basket)
        {
            baskets.Remove(basket);

            if (baskets.Count == 0)
                Dispose();
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