using System.Collections;
using UnityEngine.Networking;
using SexyDu.OnEditor;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 에디터 전용 Texture 다운로더 작업자
    /// </summary>
    public class EditorTextureDownloader : UnityTextureDownloader, ITextureDownloader
    {
        /// <summary>
        /// Texture 다운로드 요청
        /// </summary>
        public override ITextureDownloader Request(IBinaryReceipt receipt)
        {
            EditorCoroutine.StartCoroutine(CoRequest(receipt));

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
                req.SendWebRequest();
                /// EditorCoroutine의 경우 MoveNext를 기반으로 동작하기 때문에
                /// yield return req.SendWebRequest()를 한번에 통과하여 데이터를 받아올 수 없다.
                /// 하여 아래와 같이 request가 완료되었는지를 매 프레임 확인하여 수행한다.
                do
                {
                    yield return null;
                } while (!req.isDone);

                TextureResponse res = MakeResponse(req);

                callback?.Invoke(res);
            }
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// * Editor에선 기본적으로 response header를 포함해서 가져온다.
        /// </summary>
        protected override TextureResponse MakeResponse(UnityWebRequest target)
        {
            return new TextureResponse(DownloadHandlerTexture.GetContent(target), target.responseCode, target.error, ToRESTResult(target.result), target.GetResponseHeaders());
        }
    }
}