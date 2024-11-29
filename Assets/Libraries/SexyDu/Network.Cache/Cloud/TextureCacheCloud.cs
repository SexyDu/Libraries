using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Network
{
    public class TextureCloud
    {
        private readonly Dictionary<string, TextureSection> sections = new();

        public TextureCloud Request(ITextureCacheBasket basket, IBinaryReceipt receipt)
        {
            if (sections.ContainsKey(receipt.uri.AbsoluteUri))
            {
                sections[receipt.uri.AbsoluteUri].AddBasket(basket);
            }
            else
            {
                TextureSection section = new TextureSection();
                section.Load(new TextureCache(), receipt);
                sections[section.url] = section;
            }
            return this;
        }

        public void Remove(string url)
        {
            if (sections.ContainsKey(url))
                sections[url].Dispose();
            sections.Remove(url);
        }
    }

    public class TextureSection : IDisposable
    {
        public string url { get; private set; }
        public Texture2D texture
        {
            get;
            private set;
        }

        public void Dispose()
        {
            foreach (var basket in baskets)
                basket.Dispose();
            baskets.Clear();
        }

        public TextureSection Load(ITextureDownloader loader, IBinaryReceipt receipt)
        {
            url = receipt.uri.AbsoluteUri;

            IDisposable disposable = loader.Request(receipt).Subscribe(res =>
            {
                SetTexture(res.tex);
            });

            return this;
        }

        private void SetTexture(Texture2D texture)
        {
            this.texture = texture;

            Distribute();
        }

        private readonly List<ITextureCacheBasket> baskets = new();

        public void AddBasket(ITextureCacheBasket basket)
        {
            baskets.Add(basket);

            if (texture != null)
                basket.Pour(texture);
        }

        public void RemoveBasket(ITextureCacheBasket basket)
        {
            baskets.Remove(basket);
        }

        private void Distribute()
        {
            foreach (var basket in baskets)
                basket.Pour(texture);
        }
    }
}

