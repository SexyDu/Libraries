using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// RESTful API 테스터 Scriptable Object
    /// * Scriptable Object로 한 이유는 다시 윈도우를 활성화 할 떄 이 전에 사용했던 정보를 유지하기 위함.
    /// </summary>
    [CreateAssetMenu(fileName = "RESTAPITester", menuName = "SexyDu/Editor/NetworkTester/RESTAPITester")]
    public class RESTAPITester : BaseTester
    {
        // Resources 경로
        public const string ResourcePath = "NetworkTester/RESTAPITester";

        // 요청 정보
        public RequestInformation requestData;

        // 수신 정보
        public ResponseInformation responseData;

        public override void Initialize(EditorWindow window)
        {
            base.Initialize(window);

            InitializeSerializedObject();
        }

        public override void Clear()
        {
            base.Clear();

            requestData.Clear();
            responseData.Clear();
        }

        #region Serialized
        private SerializedObject serializedObject = null;
        private SerializedProperty headersProperty = null;
        private void InitializeSerializedObject()
        {
            /// requestData(RequestInformation)의 변수 headers를 EditorUI로 관리할 수 있도록 직렬화 가져오기
            if (serializedObject == null)
                serializedObject = new SerializedObject(this);
            if (headersProperty == null)
                headersProperty = serializedObject.FindProperty("requestData.headers");
        }
        #endregion

        /// <summary>
        /// 테스터 UI
        /// </summary>
        public override void OnEditorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            // 기본 정보 표시
            requestData.url = EditorGUILayout.TextField("Url", requestData.url);
            ButtonTemporaryUrl();
            EditorGUILayout.EndHorizontal();
            requestData.method = (NetworkMethod)EditorGUILayout.EnumPopup("Method", requestData.method);
            requestData.timeout = EditorGUILayout.IntField("Timeout", requestData.timeout);
            EditorGUILayout.PropertyField(headersProperty, new GUIContent("Headers"), true);
            // POST 또는 PATCH(PUT)인 경우 Post Data 입력 창 표시
            if (requestData.method == NetworkMethod.POST || requestData.method == NetworkMethod.PATCH)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("PostData");
                requestData.postData = EditorGUILayout.TextArea(requestData.postData, GUILayout.Height(100));
                EditorGUILayout.EndVertical();
            }

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
                EditorGUILayout.PrefixLabel("text");
                EditorGUILayout.TextArea(responseData.text, GUILayout.Height(100));
                GUILayout.Box($"{responseData.responseTime}초 소요", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("상세 로그"))
                    Debug.Log(responseData);
                GUILayout.EndVertical();
            }
        }
        /// <summary>
        /// 현제 요청 정보를 통해 요청 수행
        /// </summary>
        public void Request()
        {
            if (!IsWorking)
            {
                IRESTReceipt receipt = requestData.GetRESTReceipt();

                responseData.RecordReqeustTime();

                EditorRESTWorker worker = new EditorRESTWorker();
                worker.
                Request(receipt).
                Subscribe((res) =>
                {
                    responseData.Set(res);
                    ClearNetworker();
                    // OnGUI는 매 프레임 도는 것이 아니기 때문에 요청을 받아온 후 EditorWindow.Repaint()를 수행하여 화면을 갱신해준다.
                    Repaint();
                });

                SetNetworker(worker);
            }
            else
                EditorUtility.DisplayDialog("경고", $"이미 Network를 수행중입니다.", "OK");
        }

        /// <summary>
        /// API 요청 해더 정보
        /// </summary>
        [Serializable]
        public struct Header
        {
            public string key;
            public string value;
        }

        /// <summary>
        /// API 요청 정보
        /// </summary>
        [Serializable]
        public class RequestInformation : BaseRequest
        {
            // 헤더
            public List<Header> headers = new List<Header>();
            // 포스트 데이터
            public string postData = string.Empty;
            // 포스트 데이터 존재 여부
            public bool HasPostData => !string.IsNullOrEmpty(postData);

            /// <summary>
            /// 클리어
            /// </summary>
            public override void Clear()
            {
                base.Clear();

                if (headers != null)
                    headers.Clear();

                postData = string.Empty;
            }

            /// <summary>
            /// 현재 헤더 반환
            /// </summary>
            public Dictionary<string, string> GetHeaders()
            {
                if (headers != null && headers.Count > 0)
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        result.Add(headers[i].key, headers[i].value);
                    }

                    return result;
                }
                else
                    return null;
            }

            /// <summary>
            /// API 요청 접수증 반환
            /// </summary>
            public IRESTReceipt GetRESTReceipt()
            {
                var receipt = new RESTReceipt(method)
                    .SetUri(url)
                    .SetTimeout(timeout)
                    .SetHeaders(GetHeaders());
                if (HasPostData)
                    receipt.SetBody(postData);

                return receipt;
            }
        }

        /// <summary>
        /// API 수신 정보
        /// </summary>
        [Serializable]
        public class ResponseInformation : BaseResponse
        {
            // 수신 데이터
            public string text = string.Empty;

            public void Set(IResponse<string> response)
            {
                base.Set(response);

                text = response.content;
            }

            public override void Clear()
            {
                base.Clear();

                text = string.Empty;
            }

            public override string ToString()
            {
                return string.Format("{0}\n- text : {1}", base.ToString(), text);
            }
        }

        #region Temporary Url
        // 테스트용 임시 Url
        private const string TemporaryUrl = "https://drive.google.com/uc?export=download&id=12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA";
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