using System.Collections;
using UnityEngine.Networking;
using SexyDu.OnEditor;
using System;
using UnityEngine;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 에디터 전용 Texture 다운로더 작업자
    /// </summary>
    public class EditorTextureDownloader : UnityTextureDownloader, ITextureDownloader
    {
        public override void Dispose()
        {
            base.Dispose();

            if (coroutine != null)
            {
                coroutine.Dispose();
                coroutine = null;
            }
        }

        // 작업 코루틴 관리자
        private IDisposable coroutine = null;
        // 작업 중 여부
        public override bool IsWorking => coroutine != null;

        /// <summary>
        /// Texture 다운로드 요청
        /// </summary>
        public override ITextureDownloader Request(IBinaryReceipt receipt)
        {
            if (IsWorking)
                throw new InvalidOperationException("이미 작업중입니다. 요청 전 작업 확인 처리를 하거나 중단 처리(Dispose)를 수행하세요.");
                
            coroutine = EditorCoroutine.StartCoroutine(CoRequest(receipt));

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

                Notify(req);
            }

            Terminate();
        }

        /// <summary>
        /// UnityWebRequest의 수신 데이터를 기반으로 수신 데이터 재구성 및 반환
        /// * Editor에선 기본적으로 response header를 포함해서 가져온다.
        /// </summary>
        protected override IResponse<Texture2D> MakeResponse(UnityWebRequest target)
        {
            return new Response<Texture2D>(DownloadHandlerTexture.GetContent(target), target.responseCode, target.error, ToRESTResult(target.result), target.GetResponseHeaders());
        }
    }
}