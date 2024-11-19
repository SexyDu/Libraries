using System.Collections;
using UnityEngine.Networking;
using SexyDu.Tool;

namespace SexyDu.Network
{
    /// <summary>
    /// [PostData 미사용] REST API 워커
    /// </summary>
    public class SexyRESTWorker : UnityRESTWorker
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

        /// <summary>
        /// API 요청 수행
        /// </summary>
        public SexyRESTWorker Request(UnityRESTReceipt receipt)
        {
            MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }
        /// <summary>
        /// API 요청 수행 코루틴
        /// </summary>
        private IEnumerator CoRequest(UnityRESTReceipt receipt)
        {
            using (UnityWebRequest req = MakeUnityWebRequest(receipt))
            {
                // 타임아웃 설정
                SetTimeout(req, receipt.timeout);
                // 헤더 설정
                SetRequestHeaders(req, receipt.headers);

                // 요청 전달
                yield return req.SendWebRequest();

                RESTResponse res = MakeRESTResponse(req);

                callback?.Invoke(res);
            }
        }
    }

    /// <summary>
    /// [PostData 사용] REST API 워커
    /// </summary>
    public class SexyPostableRESTWorker : UnityRESTWorker
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
        /// API 요청 수행
        /// </summary>
        public SexyPostableRESTWorker Request(UnityPostableRESTReceipt receipt)
        {
            MonoHelper.StartCoroutine(CoRequest(receipt));

            return this;
        }
        /// <summary>
        /// API 요청 수행 코루틴
        /// </summary>
        private IEnumerator CoRequest(UnityPostableRESTReceipt receipt)
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

                // 타임아웃 설정
                SetTimeout(req, receipt.timeout);
                // 헤더 설정
                SetRequestHeaders(req, receipt.headers);

                // 요청 전달
                yield return req.SendWebRequest();

                RESTResponse res = MakeRESTResponse(req);

                callback?.Invoke(res);
            }
        }
    }
}