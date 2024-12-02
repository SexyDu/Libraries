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
            entry = cloud.Request<Texture2D>(new CacheReceipt().SetUri(url).SetEncryptor(encryptor)).AddBasket(this);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            if (texture != null)
            {
                Object.Destroy(texture);
            }

            Dispose();

            cloud = null;
        }

        private ICacheCloud cloud = null;
        private string url;
        private ICacheEntry entry = null;

        [SerializeField] private Texture2D texture = null;
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

        public void Pour(object obj)
        {
            Debug.LogFormat("Pour");
            texture = obj as Texture2D;
        }

        public void OnBrokenEntry()
        {
            texture = null;
            entry = null;
        }
    }
}
