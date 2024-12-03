using System;
using System.Collections.Generic;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시 클라우드
    /// </summary>
    public partial class CacheCloud : ICacheCloud//(IRequestableCacheEntry, IRemovableCacheEntry)
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

    /// <summary>
    /// 캐시 엔트리
    ///  * 하나의 캐시 오브젝트가 담길 저장소
    /// </summary>
    public class CacheEntry : ICacheEntry
    {
        // URL
        public readonly string url = null;
        // 타입
        public readonly Type type = null;
        // 데이터
        public object data
        {
            get;
            private set;
        }
        public string Url => url;
        public Type Type => type;

        // 관리자
        public IRemovableCacheEntry manager = null;

        public CacheEntry(string url, Type type)
        {
            this.url = url;
            this.type = type;
        }

        // Dispose 여부
        /// baskets이 null이면 dispose 된 것으로 간주
        public bool IsDisposed => baskets == null;

        public void Dispose()
        {
            // 이미 Dispose 된 경우 리턴
            if (IsDisposed)
                return;

            if (baskets != null)
            {
                // 모든 바스켓에 엔트리가 파괴됨을 노티
                foreach (var basket in baskets)
                    basket.OnBrokenEntry();
                // 바스켓 클리어
                baskets.Clear();
                baskets = null;
            }

            if (data != null)
            {
                // 유니티 오브젝트인 경우 파괴
                DestroyIfUnityObject(data);
                data = null;
            }

            // 관리자에 엔트리 삭제 요청
            manager.Remove(type, url);
            manager = null;
        }

        /// <summary>
        /// 관리자 설정
        /// </summary>
        public CacheEntry Set(IRemovableCacheEntry manager)
        {
            this.manager = manager;
            return this;
        }
        /// <summary>
        /// 데이터 설정
        /// </summary>
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
                // 이미 Dispose 된 경우 파괴
                DestroyIfUnityObject(data);

            return this;
        }

        // 캐시 데이터를 받아먹을 바스켓 리스트
        private List<ICacheBasket> baskets = new();
        /// <summary>
        /// 바스켓 추가
        /// </summary>
        public ICacheEntry AddBasket(ICacheBasket basket)
        {
            if (IsDisposed)
                return null;

            baskets.Add(basket);

            // 데이터가 있는 경우 배포
            if (data != null)
                basket.Pour(data);

            return this;
        }

        /// <summary>
        /// 바스켓 제거
        /// </summary>
        public ICacheEntry RemoveBasket(ICacheBasket basket)
        {
            if (IsDisposed)
                return null;

            baskets.Remove(basket);

            // 바스켓이 없는 경우 Dispose
            if (baskets.Count == 0)
                Dispose();

            return this;
        }

        /// <summary>
        /// 배포
        /// </summary>
        private void Distribute()
        {
            foreach (var basket in baskets)
                basket.Pour(data);
        }

        /// <summary>
        /// 유니티 오브젝트인 경우 파괴
        /// </summary>
        private void DestroyIfUnityObject(object obj)
        {
            if (obj is UnityEngine.Object unityObject)
                UnityEngine.Object.Destroy(unityObject);
        }
    }
}