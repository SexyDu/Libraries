using System;

namespace SexyDu.Network.Cache
{
    // Dispose에서 entry에서 RemoveBasket(자기 자신) 호출
    public interface ICacheBasket : IDisposable
    {
        public void Pour(object obj);

        public void OnBrokenEntry();
    }
}