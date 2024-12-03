using System;
using UnityEngine;
using UnityEditor;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// RESTful API 테스터 Scriptable Object
    /// * Scriptable Object로 한 이유는 다시 윈도우를 활성화 할 떄 이 전에 사용했던 정보를 유지하기 위함.
    /// </summary>
    [CreateAssetMenu(fileName = "TextureDownloadTester", menuName = "SexyDu/Editor/NetworkTester/TextureDownloadTester")]
    public class TextureDownloadTester : BaseTester
    {
        // Resources 경로
        public const string ResourcePath = "NetworkTester/TextureDownloadTester";
        
        // 요청 정보
        public RequestInformation requestData;
        // 수신 정보
        public ResponseInformation responseData;

        public override void Release()
        {
            base.Release();
            // Texture는 크기가 클 수도 있으니 윈도우가 꺼질 때 파괴되도록 한다.
            responseData.ClearTexture();
        }

        public override void Clear()
        {
            base.Clear();

            requestData.Clear();
            responseData.Clear();
        }

        /// <summary>
        /// 테스터 UI
        /// </summary>
        public override void OnEditorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            requestData.url = EditorGUILayout.TextField("Url", requestData.url);
            ButtonTemporaryUrl();
            EditorGUILayout.EndHorizontal();
            requestData.timeout = EditorGUILayout.IntField("Timeout", requestData.timeout);

            if (GUILayout.Button("요청"))
            {
                responseData.Clear();
                Request();
            }
            // 수신 데이터가 비어있지 않은 경우
            if (!responseData.IsEmpty)
            {
                EditorGUILayout.Space(10);
                GUILayout.BeginVertical();
                GUILayout.Box("Response", GUILayout.ExpandWidth(true));
                EditorGUILayout.LongField("code", responseData.code);
                if (!responseData.IsSuccess)
                    EditorGUILayout.TextField("* error", responseData.error);
                EditorGUILayout.ObjectField("texture", responseData.tex, typeof(Texture2D), true);
                GUILayout.Box($"{responseData.responseTime}초 소요", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("상세 로그"))
                    Debug.Log(responseData);
                GUILayout.EndVertical();
            }
        }
        /// <summary>
        /// 현제 요청 정보를 통해 요청 수행
        /// </summary>
        private void Request()
        {
            if (!IsWorking)
            {
                IBinaryReceipt receipt = requestData.GetReceipt();

                responseData.RecordReqeustTime();

                EditorTextureDownloader downloader = new EditorTextureDownloader();
                downloader.
                Request(receipt).
                Subscribe((res) =>
                {
                    responseData.Set(res);
                    ClearNetworker();
                    // OnGUI는 매 프레임 도는 것이 아니기 때문에 요청을 받아온 후 EditorWindow.Repaint()를 수행하여 화면을 갱신해준다.
                    Repaint();
                    // 이상하게 데이터가 ScriptableObject Asset에 저장되지 않는 것 같아서 Dirty 수행
                    EditorUtility.SetDirty(this);
                });

                SetNetworker(downloader);
            }
            else
                EditorUtility.DisplayDialog("경고", $"이미 Network를 수행중입니다.", "OK");
        }

        /// <summary>
        /// API 요청 정보
        /// </summary>
        [Serializable]
        public class RequestInformation : BaseRequest
        {
            /// <summary>
            /// API 요청 접수증 반환
            /// </summary>
            public IBinaryReceipt GetReceipt()
            {
                return new BinaryReceipt().SetUri(url).SetTimeout(timeout);
            }
        }

        /// <summary>
        /// API 수신 정보
        /// </summary>
        [Serializable]
        public class ResponseInformation : BaseResponse
        {
            // 수신 데이터
            public Texture2D tex = null;

            public void Set(IResponse<Texture2D> response)
            {
                base.Set(response);

                tex = response.content;
            }

            public override void Clear()
            {
                base.Clear();

                ClearTexture();
            }

            public void ClearTexture()
            {
                if (tex != null)
                {
                    DestroyImmediate(tex);
                    tex = null;
                }
            }
        }

        #region Temporary Url
        // 테스트용 임시 Url
        private const string TemporaryUrl = "https://pm.pstatic.net/resources/asset/sp_main.b27083b1.png";
        /// <summary>
        /// 지정된 임시 Url로 설정
        /// </summary>
        protected override void SetTemporaryUrl()
        {
            requestData.url = TemporaryUrl;
        }
        #endregion
    }
}