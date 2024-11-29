using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;

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
        public IRESTWorker Request(IRESTReceipt receipt)
        {
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

    /// <summary>
    /// [PostData 사용] SexyDu의 MonoHelper 기반의 REST API 워커
    /// </summary>
    public class SexyPostableRESTWorker : UnityRESTWorker, IPostableRESTWorker
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public SexyPostableRESTWorker() { }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="includeResponseHeaders">수신 데이터에 ResponseHeaders 포함 여부</param>
        public SexyPostableRESTWorker(bool includeResponseHeaders) : base(includeResponseHeaders) { }

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
        // 작업 중 여부
        public override bool IsWorking => requestCommander != null;

        /// <summary>
        /// API 요청 수행
        /// </summary>
        public IPostableRESTWorker Request(IPostableRESTReceipt receipt)
        {
            requestCommander = MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }
        /// <summary>
        /// API 요청 수행 코루틴
        /// </summary>
        private IEnumerator CoRequest(IPostableRESTReceipt receipt)
        {
            using (UnityWebRequest req = MakeUnityWebRequest(receipt))
            {
                /// 아래 내용은 receipt.GetUnityWebRequest함수 내부(UnityWebRequest)에서 수행되기 때문에 일단 동작시키지 않는다
#if false
                // post data를 UTF8 byte 형식으로 변경하여 업로드
                if (!string.IsNullOrEmpty(receipt.body))
                {
                    byte[] bytes = new UTF8Encoding().GetBytes(receipt.body);
                    req.uploadHandler = new UploadHandlerRaw(bytes);
                }
#endif

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