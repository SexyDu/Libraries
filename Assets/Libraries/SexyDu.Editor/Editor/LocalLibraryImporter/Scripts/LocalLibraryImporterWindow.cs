using UnityEditor;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 로컬라이브러리 임포터 윈도우
    /// </summary>
    public partial class LocalLibraryImporterWindow : EditorWindow
    {
        private void OnEnable()
        {
            SettingGUIStyles();

            if (target == null)
                target = Resources.Load<TargetFolder>(TargetFolder.ResourcePath);

            if (source == null)
                source = Resources.Load<SourceData>(SourceData.ResourcePath);
        }

        [MenuItem("SexyDu/LocalLibraryImporter")]
        static void Open()
        {
            LocalLibraryImporterWindow window = GetWindow<LocalLibraryImporterWindow>();
            window.titleContent = new GUIContent("LocalLibraryImporter");
            window.minSize = new Vector2(100f, 100f);
            window.Initialize();

            window.Show(true);
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
        private void Initialize()
        {
            Debug.LogFormat("Initialize");
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
        // 대상 폴더 에셋 정보
        private TargetFolder target = null;

        // 원본 정보
        private SourceData source = null;


        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            // 대상 폴더 UI
            OnGUITarget();

            EditorGUILayout.Space(20);
            // 원본 정보 UI
            OnGUISource();
        }
        /// <summary>
        /// 대상 폴더 UI 함수
        /// </summary>
        private void OnGUITarget()
        {
            EditorGUILayout.LabelField("대상", titleStyle);
            target.folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("TargetFolder", target.folderAsset, typeof(DefaultAsset), true);

            if (target.UnsyncedFolderAsset)
            {
                target.SyncronizeFolderAsset();
                target.SetPath();
            }

            if (target.HasTargetPath)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("에셋경로", GUILayout.Width(77));
                EditorGUILayout.SelectableLabel(target.assetPath, GUILayout.MinWidth(40), GUILayout.Height(18));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("시스템경로", GUILayout.Width(77));
                EditorGUILayout.SelectableLabel(target.systemPath, GUILayout.MinWidth(40), GUILayout.Height(18));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Sample 제거"))
                {
                    target.RemoveSample();
                    RefreshEditor();
                    EditorUtility.DisplayDialog("성공", $"샘플을 제거했습니다.", "OK");
                }
                EditorGUI.indentLevel--;
            }
        }
        /// <summary>
        /// 원본 정보 UI
        /// </summary>
        private void OnGUISource()
        {
            EditorGUILayout.LabelField("원본", titleStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("경로", GUILayout.Width(77));
            source.path = EditorGUILayout.TextField(source.path);
            if (GUILayout.Button("로드", GUILayout.Width(77)))
                source.LoadUnityFileSystems();
            EditorGUILayout.EndHorizontal();
            if (source.UnsyncedPath)
            {
                source.SyncronizePath();
                source.ClearFileSystems();
            }

            if (source.HasFileSystems)
            {
                OnGUIFileSystems();
            }
        }
        /// <summary>
        /// 원본 파일 시스템 UI
        /// </summary>
        private void OnGUIFileSystems()
        {
            for (int i = 0; i < source.fileSystems.Length; i++)
            {
                source.fileSystems[i].selected = EditorGUILayout.ToggleLeft(source.fileSystems[i].name, source.fileSystems[i].selected);
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("전체 선택"))
                source.SelectAll(true);
            if (GUILayout.Button("전체 해제"))
                source.SelectAll(false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            target.keepMeta = EditorGUILayout.ToggleLeft("meta 유지", target.keepMeta);
            if (target.keepMeta)
            {
                target.deleteEmptyFolder = EditorGUILayout.ToggleLeft("empty 폴더 삭제", target.deleteEmptyFolder);
            }

            if (GUILayout.Button("가져오기"))
            {
                Debug.LogFormat("가져오기");
                target.Bring(source.GetSelectedFileSystems());
                RefreshEditor();

                EditorUtility.DisplayDialog("성공", $"가져오기를 완료 했습니다.", "OK");
            }
        }

        /// <summary>
        /// 에디터 리로드
        /// </summary>
        private void RefreshEditor()
        {
            // 에셋 데이터베이스 새로 고침 (새로운 파일이나 변경된 파일을 감지)
            AssetDatabase.Refresh();

            /// 위 Refresh 시 스크립트에 변경이 있는 경우에 알아서 아래 동작이 수행되기 때문에 비활성화
            // // 스크립트 재컴파일 요청
            // EditorUtility.RequestScriptReload();
        }
    }
}