using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;

namespace SexyDu.Network
{
    /// <summary>
    /// SexyDu의 MonoHelper 기반의 byte array 다운로더
    /// </summary>
    public class SexyBytesDownloader : UnityBytesDownloader, IBytesDownloader
    {
        /// <summary>
        /// 해제
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (requestCommander != null)
            {
                requestCommander.Dispose();
                requestCommander = null;
            }
        }

        // 요청 코루틴 관리자
        private CoroutineCommander requestCommander = null;

        /// <summary>
        /// 요청 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>자기 자신</returns>
        public override IBytesDownloader Request(IBinaryReceipt receipt)
        {
            requestCommander = MonoHelper.StartCoroutine(CoRequest(receipt));

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

                // 옵저버에 노티
                Notify(req);
            }
        }
    }
}