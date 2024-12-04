using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시 클라우드
    /// </summary>
    public partial class CacheCloud : ICacheCloud
    {
        // 캐시 엔트리 딕셔너리
        private readonly Dictionary<string, CacheEntry> entries = new();
        /// <summary>
        /// 캐시 엔트리 존재 여부 확인
        /// </summary>
        public bool HasEntry<T>(string url) where T : class => HasEntry(GetKey<T>(url));
        /// <summary>
        /// 캐시 엔트리 반환
        /// </summary>
        private bool HasEntry(string key) => entries.ContainsKey(key);
        /// <summary>
        /// 캐시 엔트리 반환
        /// </summary>
        public ICacheEntry GetEntry<T>(string url) where T : class
        {
            string key = GetKey<T>(url);
            if (HasEntry(key))
                return entries[key];
            else
                return null;
        }

        /// <summary>
        /// 캐시 엔트리 키 반환
        /// </summary>
        private string GetKey<T>(string url)
        {
            return GetKey(typeof(T), url);
        }
        /// <summary>
        /// 캐시 엔트리 키 반환
        /// </summary>
        private string GetKey(Type type, string url)
        {
            return string.Format("{0}:{1}", type.Name, url);
        }

        /// <summary>
        /// 캐시 엔트리 요청
        /// </summary>
        public ICacheEntry Request<T>(ICacheReceipt receipt) where T : class
        {
            Type requestedType = typeof(T);
            // Cloud에서 지원하지 않는 타입 거르기
            if (IsNotSupported(requestedType))
                return null;
            // 캐시 엔트리 키
            string key = GetKey(requestedType, receipt.uri.AbsoluteUri);

            // 키에 해당하는 캐시 엔트리가 없으면 생성 및 요청
            if (!HasEntry(key))
            {
                // 생성 및 딕셔너리 추가
                CacheEntry entry = new CacheEntry(key).Set(this);
                entries[key] = entry;

                // 요청 및 캐시 엔트리 설정
                new SexyCache<T>().Request(receipt).Subscribe(res =>
                {
#if true
                    if (!entry.IsDisposed)
                        entry.Set(res);
                    else
                        res.Release();
#else
                    if (res.IsSuccess)
                    {
                        if (!entry.IsDisposed)
                            entry.Set(res);
                        else
                            res.Release();
                    }
                    else
                    {
                        /// TODO: 실패일 경우 원인 파악하고 Dictionary에서 삭제 코드 넣자
                        entry.Dispose();
                    }
#endif
                });
            }

            // 키에 해당하는 캐시 엔트리 반환
            return entries[key];
        }

        /// <summary>
        /// 캐시 엔트리 삭제
        /// </summary>
        public void Remove(string key)
        {
            if (entries.ContainsKey(key))
            {
                if (!entries[key].IsDisposed)
                    entries[key].Dispose();
                entries.Remove(key);
            }
        }
        /// <summary>
        /// 캐시 엔트리 삭제
        /// </summary>
        public void Remove<T>(string url) => Remove(GetKey<T>(url));
        /// <summary>
        /// 캐시 엔트리 삭제
        /// </summary>
        public void Remove(Type type, string url) => Remove(GetKey(type, url));

        /// <summary>
        /// 지원하지 않는 타입인지 확인
        /// </summary>
        private bool IsNotSupported(Type type)
        {
            if (type == typeof(Sprite))
            {
                Debug.LogError($"[SpriteContent 대체] CacheCloud에서 Sprite 해제 시 이슈(Sprite.texture 해제 문제)가 있습니다. SpriteContent를 사용하세요.");
                return true;
            }
            return false;
        }
    }
}