using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SexyDu.Network.Editor
{
    public class SexyNetworkWindow : EditorWindow
    {
        private const string URL = "https://drive.google.com/uc?export=download&id=12IJ4bLTWvOz3trgNhYu8lAu3r_0aDAUA";

        private void OnEnable()
        {
            SettingGUIStyles();
        }

        private void OnDestroy()
        {
            // 여기서 리소스 삭제
            Debug.LogFormat("SexyNetworkWindow 창 닫힘");
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

        EditorRESTWorker networker = new EditorRESTWorker();
        private string responseText = string.Empty;

        private void OnGUI()
        {
            if (GUILayout.Button("실행"))
            {
                TEST();
            }
            if (!string.IsNullOrEmpty(responseText))
            {
                GUILayout.Box(responseText);
            }
        }

        private void TEST()
        {
            networker.
                Request(new UnityRESTReceipt(RESTMethod.GET).SetUri(URL)).
                Subscribe((res) =>
                {
                    responseText = res.text;
                });
        }
    }

    public class EditorRESTWorker : UnityRESTWorker
    {
        public bool working
        {
            get;
            private set;
        }

        public EditorRESTWorker Request(IRESTReceipt receipt)
        {
            if (working)
            {
                EditorUtility.DisplayDialog("경고", $"현재 요청을 수행중입니다.", "OK");
            }
            else
            {
                UnityWebRequest req = MakeUnityWebRequest(receipt);

                EditorCoroutine.StartCoroutine(CoRequest(req, receipt.timeout, receipt.headers));
            }

            return this;
        }

        public EditorRESTWorker Request(IPostableRESTReceipt receipt)
        {
            if (working)
            {
                EditorUtility.DisplayDialog("경고", $"현재 요청을 수행중입니다.", "OK");
            }
            else
            {
                UnityWebRequest req = MakeUnityWebRequest(receipt);

                EditorCoroutine.StartCoroutine(CoRequest(req, receipt.timeout, receipt.headers));
            }

            return this;
        }

        private IEnumerator CoRequest(UnityWebRequest req, int timeout, Dictionary<string, string> headers)
        {
            working = true;

            // 타임아웃 설정
            SetTimeout(req, timeout);
            // 헤더 설정
            SetRequestHeaders(req, headers);

            // 요청 전달
            req.SendWebRequest();
            /// EditorCoroutine의 경우 MoveNext를 기반으로 동작하기 때문에
            /// yield return req.SendWebRequest()를 한번에 통과하여 데이터를 받아올 수 없다.
            /// 하여 아래와 같이 request가 완료되었는지를 매 프레임 확인하여 수행한다.
            while (!req.isDone)
            {
                yield return null;
            }

            callback?.Invoke(MakeRESTResponse(req));

            req.Dispose();

            working = false;
        }
    }

    public abstract class EditorBinaryWorker
    {
        public bool working
        {
            get;
            protected set;
        }

        public EditorBinaryWorker Request(string url)
        {
            return Request(new Uri(url));
        }

        public EditorBinaryWorker Request(Uri uri)
        {
            if (working)
                EditorUtility.DisplayDialog("경고", $"현재 요청을 수행중입니다.", "OK");
            else
                EditorCoroutine.StartCoroutine(CoRequest(uri));

            return this;
        }

        protected virtual IEnumerator CoRequest(Uri uri)
        {
            working = true;
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                req.SendWebRequest();

                /// EditorCoroutine의 경우 MoveNext를 기반으로 동작하기 때문에
                /// yield return req.SendWebRequest()를 한번에 통과하여 데이터를 받아올 수 없다.
                /// 하여 아래와 같이 request가 완료되었는지를 매 프레임 확인하여 수행한다.
                while (!req.isDone)
                {
                    yield return null;
                }
            }

            working = false;
        }
    }

    public class EditorCoroutine
    {
        public static EditorCoroutine StartCoroutine(IEnumerator _routine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Start();
            return coroutine;
        }

        readonly IEnumerator routine;
        private EditorCoroutine(IEnumerator _routine) => routine = _routine;

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (mode == UnityEngine.SceneManagement.LoadSceneMode.Single)
                EditorApplication.update -= Update;
        }

        private void Start()
        {
            EditorApplication.update += Update;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void Stop()
        {
            EditorApplication.update -= Update;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            if (!routine.MoveNext()) Stop();
        }
    }
}