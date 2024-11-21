using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using SexyDu.OnEditor;

namespace SexyDu.Network.Editor
{
    public class SexyNetworkWindow : EditorWindow
    {
        // . // 여기 이제 Texture 기능하고 코드 정리 및 주석작성하자 !!
        // private const string URL = "https://drive.google.com/uc?export=download&id=12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA";

        private SerializedObject serializedObject;
        private SerializedProperty headersProperty;

        private void OnEnable()
        {
            SettingGUIStyles();

            serializedObject = new SerializedObject(this);
            headersProperty = serializedObject.FindProperty("requestInfo.headers");
            if (headersProperty == null)
            {
                Debug.LogError("headersProperty가 제대로 설정되지 않았습니다.");
            }
        }

        private void OnDestroy()
        {
            // 여기서 리소스 삭제
            Debug.LogFormat("SexyNetworkWindow 창 닫힘");

            responseREST.Clear();
        }

        #region GUIStyle
        // 타이틀 스타일
        private GUIStyle titleStyle = null;
        /// <summary>
        /// GUI 스타일 설정
        /// </summary>
        private void SettingGUIStyles()
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle();
                titleStyle.fontSize = 15;
                titleStyle.fontStyle = FontStyle.Normal;
                titleStyle.normal.textColor = Color.white;
            }
        }
        #endregion

        [MenuItem("SexyDu/NetworkTester")]
        static void Open()
        {
            SexyNetworkWindow window = GetWindow<SexyNetworkWindow>();
            window.titleContent = new GUIContent("NetworkTester");
            window.minSize = new Vector2(100f, 100f);
            // window.Initialize();

            window.Show(true);
        }

        #region Networker
        INetworker networker = null;

        private bool working => networker != null;

        private void SetNetworker(INetworker networker)
        {
            this.networker = networker;
        }

        public void ClearNetworker()
        {
            SetNetworker(null);
        }
        #endregion

        private string responseText = string.Empty;

        private void OnGUI()
        {
            serializedObject.Update();

            GUI.enabled = !working;

            OnGUIRequestREST();

            if (!string.IsNullOrEmpty(responseText))
            {
                GUILayout.Box(responseText);
            }

            serializedObject.ApplyModifiedProperties();
        }

        [SerializeField] private RequestInformation requestInfo = new RequestInformation();

        #region REST
        [SerializeField] private ResponseText responseREST = new ResponseText();
        private void OnGUIRequestREST()
        {
            EditorGUILayout.LabelField("Request", titleStyle);
            requestInfo.url = EditorGUILayout.TextField("Url", requestInfo.url);
            requestInfo.method = (NetworkMethod)EditorGUILayout.EnumPopup("Method", requestInfo.method);
            requestInfo.timeout = EditorGUILayout.IntField("Timeout", requestInfo.timeout);
            EditorGUILayout.PropertyField(headersProperty, new GUIContent("Headers"), true);
            if (requestInfo.method == NetworkMethod.POST || requestInfo.method == NetworkMethod.PATCH)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("PostData");
                requestInfo.postData = EditorGUILayout.TextArea(requestInfo.postData, GUILayout.Height(100));
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("요청"))
            {
                responseREST.Clear();
                RequestREST();
            }

            if (!responseREST.IsEmpty)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Response", titleStyle);
                GUILayout.Box($"{responseREST.time}초 소요", GUILayout.ExpandWidth(false));
                EditorGUILayout.LongField("code", responseREST.code);
                if (!responseREST.IsSuccess)
                    EditorGUILayout.TextField("* error", responseREST.error);
                EditorGUILayout.PrefixLabel("PostData");
                EditorGUILayout.TextArea(responseREST.text, GUILayout.Height(100));
            }
        }
        private void RequestREST()
        {
            if (networker == null)
            {
                IRESTReceipt receipt = null;
                if (requestInfo.HasPostData)
                {
                    receipt
                        = new RESTReceipt(requestInfo.method)
                        .SetUri(requestInfo.url)
                        .SetTimeout(requestInfo.timeout)
                        .SetHeaders(requestInfo.GetHeaders());
                }
                else
                {
                    receipt
                        = new PostableRESTReceipt(requestInfo.method)
                        .SetUri(requestInfo.url)
                        .SetTimeout(requestInfo.timeout)
                        .SetHeaders(requestInfo.GetHeaders())
                        .SetBody(requestInfo.postData);
                }

                EditorRESTWorker worker = new EditorRESTWorker();
                responseREST.Ready();
                worker.
                Request(receipt).
                Subscribe((res) =>
                {
                    responseREST.Set(res);
                    ClearNetworker();
                    // OnGUI는 매 프레임 도는 것이 아니기 때문에 요청을 받아온 후 EditorWindow.Repaint()를 수행하여 화면을 갱신해준다.
                    Repaint();
                });

                networker = worker;
            }
            else
                EditorUtility.DisplayDialog("경고", $"이미 Network를 수행중입니다.", "OK");
        }
        #endregion


        [Serializable]
        public struct Header
        {
            public string key;
            public string value;
        }

        [Serializable]
        public class RequestInformation
        {
            public string url = string.Empty;
            public NetworkMethod method;
            public int timeout;
            public List<Header> headers = new List<Header>();
            public string postData = string.Empty;

            public bool HasPostData => !string.IsNullOrEmpty(postData);

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
        }

        [Serializable]
        public class ResponseBase
        {
            // 수신 코드
            public long code = long.MinValue;
            // 수신 에러
            public string error = string.Empty;
            // 수신 결과
            public NetworkResult result = NetworkResult.Unknown;
            // 수신 헤더 (딕셔너리)
            public Dictionary<string, string> headers = null;

            // 비어있는 데이터인지 여부
            public bool IsEmpty => result == NetworkResult.Unknown;
            // 성공 여부
            public bool IsSuccess => result == NetworkResult.Success;

            // 요청 시간
            private DateTime dt;
            // 요청 소요 시간
            public double time = 0;

            public void Ready()
            {
                dt = DateTime.Now;
            }

            protected void Set(IResponse response)
            {
                code = response.code;
                error = response.error;
                result = response.result;
                headers = response.headers;

                time = GetPassSeconds(dt, DateTime.Now);
            }

            public virtual void Clear()
            {
                code = long.MinValue;
                error = string.Empty;
                result = NetworkResult.Unknown;
                headers = null;
            }

            private double GetPassSeconds(DateTime start, DateTime end)
            {
                TimeSpan difference = end - start;
                return difference.TotalSeconds;
            }
        }

        [Serializable]
        public class ResponseText : ResponseBase
        {
            public string text = string.Empty;

            public void Set(ITextResponse response)
            {
                base.Set(response);

                text = response.text;
            }

            public override void Clear()
            {
                base.Clear();

                text = string.Empty;
            }
        }

        [Serializable]
        public class ResponseTexture : ResponseBase
        {
            public Texture2D tex2D = null;

            public void Set(ITextureResponse response)
            {
                base.Set(response);

                tex2D = response.tex;
            }

            public override void Clear()
            {
                base.Clear();

                if (tex2D != null)
                {
                    UnityEngine.Object.Destroy(tex2D);
                    tex2D = null;
                }
            }
        }
    }

    public class EditorRESTWorker : UnityRESTWorker, IRESTWorker, IPostableRESTWorker
    {
        public IRESTWorker Request(IRESTReceipt receipt)
        {
            UnityWebRequest req = MakeUnityWebRequest(receipt);

            EditorCoroutine.StartCoroutine(CoRequest(req));

            return this;
        }

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

            callback?.Invoke(MakeResponse(req));

            req.Dispose();
        }
    }

    public class EditorTextureDownloader : UnityTextureDownloader, ITextureDownloader
    {
        public ITextureDownloader Request(IBinaryReceipt receipt)
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
                yield return req.SendWebRequest();

                TextureResponse res = MakeResponse(req);

                callback?.Invoke(res);
            }
        }
    }
}