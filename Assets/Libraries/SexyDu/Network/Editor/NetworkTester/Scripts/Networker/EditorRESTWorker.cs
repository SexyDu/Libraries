using System.Collections;
using UnityEngine.Networking;
using SexyDu.OnEditor;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 에디터 전용 REST API 작업자
    /// </summary>
    public class EditorRESTWorker : UnityRESTWorker, IRESTWorker, IPostableRESTWorker
    {
        /// <summary>
        /// 생성자
        /// * Editor에선 기본적으로 response header를 포함해서 가져온다.
        /// </summary>
        public EditorRESTWorker() : base(true)
        { }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="includeResponseHeaders"></param>
        public EditorRESTWorker(bool includeResponseHeaders) : base(includeResponseHeaders)
        { }

        /// <summary>
        /// [PostData 미포함] API 요청
        /// </summary>
        public IRESTWorker Request(IRESTReceipt receipt)
        {
            UnityWebRequest req = MakeUnityWebRequest(receipt);

            EditorCoroutine.StartCoroutine(CoRequest(req));

            return this;
        }
        /// <summary>
        /// [PostData 포함] API 요청
        /// </summary>
        public IPostableRESTWorker Request(IPostableRESTReceipt receipt)
        {
            UnityWebRequest req = MakeUnityWebRequest(receipt);

            EditorCoroutine.StartCoroutine(CoRequest(req));

            return this;
        }

        private IEnumerator CoRequest(UnityWebRequest req)
        {
            // 요청 전달
            req.SendWebRequest();
            /// EditorCoroutine의 경우 MoveNext를 기반으로 동작하기 때문에
            /// yield return req.SendWebRequest()를 한번에 통과하여 데이터를 받아올 수 없다.
            /// 하여 아래와 같이 request가 완료되었는지를 매 프레임 확인하여 수행한다.
            do
            {
                yield return null;
            } while (!req.isDone);

            Notify(req);

            req.Dispose();
        }
    }
}