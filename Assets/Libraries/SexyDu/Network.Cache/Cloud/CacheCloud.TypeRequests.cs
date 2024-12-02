using System;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    public partial class CacheCloud
    {
        private ICacheEntry Request(Type type, string key, ICacheReceipt receipt)
        {
            if (type == typeof(Texture2D))
                return RequestTexture(key, receipt);
            else if (type == typeof(Sprite))
                throw new NotImplementedException("Sprite 정의 해야함");
            else
                throw new NotSupportedException($"Not supported type: {type.Name}");
        }

        private ICacheEntry RequestTexture(string key, ICacheReceipt receipt)
        {
            if (!entries.ContainsKey(key))
            {
                CacheEntry entry = new CacheEntry(receipt.uri.AbsoluteUri, typeof(Texture2D)).Set(this);
                entries[key] = entry;

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

            return entries[key];
        }
    }
}