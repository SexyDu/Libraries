using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;

namespace SexyDu.Network
{
    /// <summary>
    /// SexyDu의 MonoHelper 기반의 Texture(2D) 다운로더
    /// </summary>
    public class SexyTextureDownloader : UnityTextureDownloader, ITextureDownloader
    {
        public ITextureDownloader Request(IBinaryReceipt receipt)
        {
            MonoHelper.StartCoroutine(CoRequest(receipt));
            
            return this;
        }

        /// <summary>
        /// API 요청 수행 코루틴
        /// </summary>
        private IEnumerator CoRequest(IBinaryReceipt receipt)
        {
            using (UnityWebRequest req = MakeUnityWebRequest(receipt))
            {
                // 요청 전달
                yield return req.SendWebRequest();

                TextureResponse res = MakeResponse(req);

                callback?.Invoke(res);
            }
        }
    }
}