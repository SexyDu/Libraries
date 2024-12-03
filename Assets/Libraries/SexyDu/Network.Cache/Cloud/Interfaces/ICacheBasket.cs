using System;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// Cache 데이터를 받는 인터페이스
    ///  * Dispose에서 속한 entry에 RemoveBasket(자기 자신) 호출
    /// </summary>
    public interface ICacheBasket : IDisposable
    {
        /// <summary>
        /// 데이터 발생 시 호출(수신)되는 함수
        /// </summary>
        public void Pour(IResponse res);
        /// <summary>
        /// 속한 entry가 파괴되었을 때 이벤트
        /// </summary>
        public void OnBrokenEntry();
    }
}