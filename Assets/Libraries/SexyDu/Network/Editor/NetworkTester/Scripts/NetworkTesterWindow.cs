using UnityEngine;
using UnityEditor;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 네트워크 테스터 윈도우
    /// </summary>
    public class NetworkTesterWindow : EditorWindow
    {
        // REST API 테스터
        private RESTAPITester restApi = null;
        // TextureDownload 테스터
        private TextureDownloadTester textureDownload = null;
        // 현재 워커가 수행중인지 여부
        private bool IsNetworking => restApi.IsWorking || textureDownload.IsWorking;

        private void OnEnable()
        {
            SettingGUIStyles();

            // REST API 테스터 ScriptableObject 가져와서 초기설정
            if (restApi == null)
                restApi = Resources.Load<RESTAPITester>(RESTAPITester.ResourcePath);
            restApi.Initialize(this);

            // TextureDownload 테스터 ScriptableObject 가져와서 초기설정
            if (textureDownload == null)
                textureDownload = Resources.Load<TextureDownloadTester>(TextureDownloadTester.ResourcePath);
            textureDownload.Initialize(this);
        }

        private void OnDestroy()
        {
            if (restApi != null)
            {
                restApi.Release();
                restApi = null;
            }
            if (textureDownload != null)
            {
                textureDownload.Release();
                textureDownload = null;
            }
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
            NetworkTesterWindow window = GetWindow<NetworkTesterWindow>();
            window.titleContent = new GUIContent("NetworkTester");
            window.minSize = new Vector2(100f, 100f);
            // window.Initialize();

            window.Show(true);
        }

        private void OnGUI()
        {
            GUI.enabled = !IsNetworking;

            networkTypeIndex = EditorGUILayout.Popup("네트워크 타입", networkTypeIndex, networkTypes);
            EditorGUILayout.Space(10);

            switch (networkTypeIndex)
            {
                case 0:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("RESTful API", titleStyle);
                    if (GUILayout.Button("Clear", GUILayout.Width(100)))
                        restApi.Clear();
                    EditorGUILayout.EndHorizontal();
                    restApi.OnEditorGUI();
                    break;
                case 1:
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Texture Downloader", titleStyle);
                    if (GUILayout.Button("Clear", GUILayout.Width(100)))
                        textureDownload.Clear();
                    EditorGUILayout.EndHorizontal();
                    textureDownload.OnEditorGUI();
                    break;
            }

            GUI.enabled = true;
        }

        #region Network Type
        public int networkTypeIndex;
        private readonly string[] networkTypes = new string[]
        {
            "RESTful API",
            "Texture Downloader"
        };
        #endregion
    }
}