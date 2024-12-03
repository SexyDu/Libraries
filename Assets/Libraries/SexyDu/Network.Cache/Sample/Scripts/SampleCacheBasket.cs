using UnityEngine;

namespace SexyDu.Network.Cache.Sample
{
    public class SampleCacheBasket : MonoBehaviour, ICacheBasket
    {
        [SerializeField] private bool useEncryptor = false;
        [SerializeField] private bool useHmac = false;

        private void OnEnable()
        {
            CacheEncryptor encryptor = null;
            if (useEncryptor)
            {
                encryptor = CacheEncryptor.Default;
                if (useHmac)
                    encryptor.UseDefaultHmac();
            }

            if (sr == null)
                entry = cloud.Request<Texture2D>(new CacheReceipt().SetUri(url).SetEncryptor(encryptor)).AddBasket(this);
            else
                entry = cloud.Request<SpriteContent>(new CacheReceipt().SetUri(url).SetEncryptor(encryptor)).AddBasket(this);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            cloud = null;
        }

        private ICacheCloud cloud = null;
        private string url;
        private ICacheEntry entry = null;

        [SerializeField] private Texture2D texture = null;
        [SerializeField] private SpriteRenderer sr = null;
        public SampleCacheBasket SetCacheCloud(ICacheCloud cloud)
        {
            this.cloud = cloud;
            return this;
        }

        public SampleCacheBasket SetUrl(string uri)
        {
            this.url = uri;
            return this;
        }

        public void Dispose()
        {
            if (entry != null)
            {
                entry?.RemoveBasket(this);
                entry = null;
            }

            texture = null;
        }

        public void Pour(IResponse res)
        {
            Debug.LogFormat("Pour");
            if (res is IResponse<Texture2D> resTex)
            {
                texture = resTex.content;
            }
            else if (res is IResponse<SpriteContent> respSp)
            {
                sr.sprite = respSp.content.sprite;
            }
        }

        public void OnBrokenEntry()
        {
            texture = null;
            entry = null;
        }
    }
}
