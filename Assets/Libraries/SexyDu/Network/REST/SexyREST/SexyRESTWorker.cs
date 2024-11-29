using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;
using System;

namespace SexyDu.Network
{
    /// <summary>
    /// [PostData 미사용] SexyDu의 MonoHelper 기반의 REST API 워커
    /// </summary>
    public class SexyRESTWorker : UnityRESTWorker, IRESTWorker
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public SexyRESTWorker() { }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="includeResponseHeaders">수신 데이터에 ResponseHeaders 포함 여부</param>
        public SexyRESTWorker(bool includeResponseHeaders) : base(includeResponseHeaders) { }

        // 요청 코루틴 관리자
        private CoroutineCommander requestCommander = null;
        // 작업 중 여부
        public override bool IsWorking => requestCommander != null;

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
        // private CoroutineCommander requestCommander = null;

        /// <summary>
        /// API 요청 수행
        /// </summary>
        public override IRESTWorker Request(IRESTReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");
                
            requestCommander = MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }
        /// <summary>
        /// API 요청 수행 코루틴
        /// </summary>
        private IEnumerator CoRequest(IRESTReceipt receipt)
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