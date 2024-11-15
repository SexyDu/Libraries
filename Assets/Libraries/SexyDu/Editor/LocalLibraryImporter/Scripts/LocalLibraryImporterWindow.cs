using UnityEditor;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{

    // TODO
    /// 1. ToDoWindow는 가져오고 나서 윈도우 잘 유지 되는데 LocalLibraryImporterWindow는 설정이 사라지거나 에러가 발생. 이유를 찾자.
    /// 2. 전부 주석 작성하자.
    public partial class LocalLibraryImporterWindow : EditorWindow
    {
        private void OnEnable()
        {
            SettingGUIStyles();
        }

        [MenuItem("SexyDu/LocalLibraryImporter")]
        static void Open()
        {
            LocalLibraryImporterWindow window = GetWindow<LocalLibraryImporterWindow>();
            window.titleContent = new GUIContent("LocalLibraryImporter");
            window.minSize = new Vector2(100f, 100f);
            // window.Initialize();

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
        private TargetFolder target = new TargetFolder();

        // 원본 정보 구조체
        private SourceData source = new SourceData();


        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            OnGUITarget();

            EditorGUILayout.Space(20);

            OnGUISource();
        }

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
                EditorGUI.indentLevel--;
            }
        }

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
                target.Bring(source.GetSelectedFileSystems());
                RefreshEditor();
            }
        }

        /// <summary>
        /// 에디터 리로드
        /// </summary>
        private void RefreshEditor()
        {
            // 에셋 데이터베이스 새로 고침 (새로운 파일이나 변경된 파일을 감지)
            AssetDatabase.Refresh();

            // // 스크립트 재컴파일 요청
            // EditorUtility.RequestScriptReload();
        }
    }
}