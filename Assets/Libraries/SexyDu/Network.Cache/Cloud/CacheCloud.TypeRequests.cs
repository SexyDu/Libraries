using System;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 타입별 캐시 요청 기능 모음
    /// </summary>
    public partial class CacheCloud
    {
        /// <summary>
        /// 타입별 캐시 요청
        /// </summary>
        /// <param name="type">캐시 타입</param>
        /// <param name="key">캐시 키</param>
        /// <param name="receipt">캐시 접수증</param>
        /// <returns>캐시 엔트리</returns>
        /// <exception cref="NotSupportedException">지원하지 않는 타입일 경우 예외 발생</exception>
        private ICacheEntry Request(Type type, string key, ICacheReceipt receipt)
        {
            if (type == typeof(Texture2D))
                return RequestTexture(key, receipt);
            else if (type == typeof(Sprite))
                return RequestSprite(key, receipt);
            else
                throw new NotSupportedException($"Not supported type: {type.Name}");
        }
        /// <summary>
        /// 텍스처 캐시 요청
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <param name="receipt">캐시 접수증</param>
        /// <returns>캐시 엔트리</returns>
        private ICacheEntry RequestTexture(string key, ICacheReceipt receipt)
        {
            if (!HasEntry(key))
            {
                CacheEntry entry = new CacheEntry(receipt.uri.AbsoluteUri, typeof(Texture2D)).Set(this);
                entries[key] = entry;

                new TextureCache().Request(receipt).Subscribe(res =>
                {
                    if (res.IsSuccess)
                    {
                        if (!entry.IsDisposed)
                        {
                            entry.Set(res.data);
                        }
                        else
                            UnityEngine.Object.Destroy(res.data);
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
        /// <summary>
        /// 스프라이트 캐시 요청
        /// </summary>
        /// <param name="key">캐시 키</param>
        /// <param name="receipt">캐시 접수증</param>
        /// <returns>캐시 엔트리</returns>
        private ICacheEntry RequestSprite(string key, ICacheReceipt receipt)
        {
            if (!HasEntry(key))
            {
                CacheEntry entry = new CacheEntry(receipt.uri.AbsoluteUri, typeof(Sprite)).Set(this);
                entries[key] = entry;

                new TextureCache().Request(receipt).Subscribe(res =>
                {
                    if (res.IsSuccess)
                    {
                        if (!entry.IsDisposed)
                        {
                            entry.Set(Sprite.Create(res.data, new Rect(0, 0, res.data.width, res.data.height), new Vector2(0.5f, 0.5f)));
                        }
                        else
                            UnityEngine.Object.Destroy(res.data);
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