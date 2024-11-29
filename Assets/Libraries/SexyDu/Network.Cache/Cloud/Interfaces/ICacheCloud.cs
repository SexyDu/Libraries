using UnityEngine;

namespace SexyDu.Network
{
    public interface ICacheCloud
    {
        public void AddBasket(ICacheBasket basket);
        public void RemoveBasket(ICacheBasket basket);
    }
}