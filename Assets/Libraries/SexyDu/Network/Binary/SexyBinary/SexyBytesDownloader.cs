using System;
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
        /// 작업 중 여부
        /// </summary>
        public override bool IsWorking => requestCommander != null;

        /// <summary>
        /// 요청 수행
        /// </summary>
        /// <param name="receipt">접수증</param>
        /// <returns>자기 자신</returns>
        public override IBytesDownloader Request(IBinaryReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");
                
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

            // 작업 종료
            Terminate();
        }
    }
}