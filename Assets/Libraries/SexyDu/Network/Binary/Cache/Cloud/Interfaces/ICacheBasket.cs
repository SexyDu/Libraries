using System;
using UnityEngine;

namespace SexyDu.Network
{
    public interface ICacheBasket : IDisposable
    {
    }

    public interface ITextureCacheBasket : ICacheBasket
    {
        public void Pour(Texture2D texture);
    }
}