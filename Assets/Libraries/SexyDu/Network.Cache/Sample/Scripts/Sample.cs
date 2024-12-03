using System;
using UnityEngine;

namespace SexyDu.Network.Cache.Sample
{
    public class Sample : MonoBehaviour
    {
        private const string imageUrl = "https://pm.pstatic.net/resources/asset/sp_main.b27083b1.png";

        [SerializeField] private SampleCacheBasket[] baskets;
        private ICacheCloud cloud = new CacheCloud();
        private void Awake()
        {
            foreach (var basket in baskets)
                basket.SetCacheCloud(cloud).SetUrl(imageUrl);
        }

        [SerializeField] private Texture2D texture = null;

        private void Clear()
        {
            if (texture != null)
            {
                UnityEngine.Object.Destroy(texture);
                texture = null;
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Load"))
            {
                Clear();

                new TextureCache()
                .Request(new CacheReceipt()
                    .SetUri(imageUrl))
                .Subscribe(res =>
                {
                    texture = res.data;
                });
            }
            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Load"))
            {
                Clear();

                new TextureCache()
                .Request(new CacheReceipt()
                    .SetUri(imageUrl)
                    .SetEncryptor(CacheEncryptor.Default))
                .Subscribe(res =>
                {
                    texture = res.data;
                });
            }
            if (GUI.Button(new Rect(200f, 0f, 100f, 100f), "Load"))
            {
                Clear();

                new TextureCache()
                .Request(new CacheReceipt()
                    .SetUri(imageUrl)
                    .SetEncryptor(CacheEncryptor.Default.UseDefaultHmac()))
                .Subscribe(res =>
                {
                    texture = res.data;
                });
            }

            if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "Entry Dispose"))
            {
                cloud.GetEntry<Texture2D>(imageUrl).Dispose();
            }
            if (GUI.Button(new Rect(100f, 100f, 100f, 100f), "GC Collect"))
            {
                GC.Collect();
            }

        }
    }
}