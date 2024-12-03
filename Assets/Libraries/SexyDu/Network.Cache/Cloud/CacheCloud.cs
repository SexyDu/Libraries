using System;
using System.Collections.Generic;

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
        public bool HasEntry<T>(string url) => HasEntry(GetKey<T>(url));
        /// <summary>
        /// 캐시 엔트리 반환
        /// </summary>
        private bool HasEntry(string key) => entries.ContainsKey(key);
        /// <summary>
        /// 캐시 엔트리 반환
        /// </summary>
        public ICacheEntry GetEntry<T>(string url)
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
        public ICacheEntry Request<T>(ICacheReceipt receipt)
        {
            Type requestedType = typeof(T);
            string key = GetKey(requestedType, receipt.uri.AbsoluteUri);

            if (entries.ContainsKey(key))
                return entries[key];
            else
                return Request(requestedType, key, receipt);
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
        /// 캐시 엔트리 삭제
        /// </summary>
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
}