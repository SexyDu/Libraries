using System;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시 클라우드 인터페이스
    /// </summary>
    public interface ICacheCloud : IRequestableCacheEntry, IRemovableCacheEntry
    {
        /// <summary>
        /// 캐시 엔트리 존재 여부 확인
        /// </summary>
        /// <typeparam name="T">캐시 타입</typeparam>
        /// <param name="url">URL</param>
        /// <returns>캐시 엔트리 존재 여부</returns>
        public bool HasEntry<T>(string url);

        /// <summary>
        /// 캐시 엔트리 반환
        /// </summary>
        /// <typeparam name="T">캐시 타입</typeparam>
        /// <param name="url">URL</param>
        /// <returns>캐시 엔트리</returns>
        public ICacheEntry GetEntry<T>(string url);
    }

    /// <summary>
    /// 캐시 엔트리 요청 인터페이스
    /// </summary>
    public interface IRequestableCacheEntry
    {
        /// <summary>
        /// 캐시 엔트리 요청
        /// </summary>
        /// <typeparam name="T">캐시 타입</typeparam>
        /// <param name="receipt">접수증</param>
        /// <returns>캐시 엔트리</returns>
        public ICacheEntry Request<T>(ICacheReceipt receipt);
    }

    /// <summary>
    /// 캐시 엔트리 삭제 인터페이스
    /// </summary>
    public interface IRemovableCacheEntry
    {
        /// <summary>
        /// 캐시 엔트리 삭제
        /// </summary>
        /// <typeparam name="T">캐시 타입</typeparam>
        /// <param name="url">URL</param>
        public void Remove<T>(string url);
        /// <summary>
        /// 캐시 엔트리 삭제
        /// </summary>
        /// <param name="type">캐시 타입</param>
        /// <param name="url">URL</param>
        public void Remove(Type type, string url);
    }

    /// <summary>
    /// 캐시 엔트리 인터페이스
    /// </summary>
    public interface ICacheEntry : IDisposable
    {
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; }
        /// <summary>
        /// 캐시 타입
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 바구니 추가
        /// </summary>
        /// <param name="basket">장바구니</param>
        /// <returns>자기 자신</returns>
        public ICacheEntry AddBasket(ICacheBasket basket);
        /// <summary>
        /// 바구니 제거
        /// </summary>
        /// <param name="basket">장바구니</param>
        /// <returns>자기 자신</returns>
        public ICacheEntry RemoveBasket(ICacheBasket basket);
    }
}