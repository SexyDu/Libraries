using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시 엔트리
    ///  * 하나의 캐시 오브젝트가 담길 저장소
    /// </summary>
    public class CacheEntry : ICacheEntry
    {
        // URL
        public readonly string key = null;
        // 데이터
        public IResponse response
        {
            get;
            private set;
        }
        public string Key => key;

        // 관리자
        public ICacheCloud manager = null;

        public CacheEntry(string key)
        {
            this.key = key;
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

            if (response != null)
            {
                // 유니티 오브젝트인 경우 파괴
                response.Release();
                response = null;
            }

            // 관리자에 엔트리 삭제 요청
            manager.Remove(key);
            manager = null;
        }

        /// <summary>
        /// 관리자 설정
        /// </summary>
        public CacheEntry Set(ICacheCloud manager)
        {
            this.manager = manager;
            return this;
        }
        /// <summary>
        /// 데이터 설정
        /// </summary>
        public CacheEntry Set(IResponse response)
        {
            if (!IsDisposed)
            {
                this.response = response;

                // basket이 있는 경우 배포
                if (baskets.Count > 0)
                    Distribute();
                // basket이 없는 경우 Dispose
                else
                    Dispose();
            }
            else
                // 이미 Dispose 된 경우 해제
                response.Release();

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
            if (response != null)
                basket.Pour(response);

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
                basket.Pour(response);
        }
    }
}