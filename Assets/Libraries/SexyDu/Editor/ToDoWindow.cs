using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

namespace SexyDu.OnEditor
{
    // . // 코드 정리 하고 주석 달자
    // 인스펙터 관련 영상 : https://www.youtube.com/watch?v=EuWFw8fT24g
    public class ToDoWindow : EditorWindow
    {
        [MenuItem("SexyDu/ToDo")]
        static void Open()
        {
            ToDoWindow window = GetWindow<ToDoWindow>();
            window.titleContent = new GUIContent("ToDo");
            window.minSize = new Vector2(100f, 100f);
            window.Initialize();

            window.Show();
        }

        private GUIStyle titleStyle = null;

        private void InitializeGUIStyle()
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle();
                titleStyle.fontSize = 15;
                titleStyle.fontStyle = FontStyle.Normal;
                titleStyle.normal.textColor = Color.white;
            }
        }

        private void OnEnable()
        {
            // Open보다 OnEnable이 더 일직 불리기 때문에 윈도우 설정에 꼭 필요한 기능은 여기에서 설정한다.
            InitializeGUIStyle();
        }

        private ComparisonProperties prev = new ComparisonProperties();

        private DefaultAsset folderAsset;
        private string assetPath = string.Empty;
        private string systemPath = string.Empty;
        private bool HasTargetFolder => folderAsset != null;
        private bool HasTargetPath => !string.IsNullOrEmpty(assetPath);

        private string sourcePath = string.Empty;
        private bool HasSourcePath => !string.IsNullOrEmpty(sourcePath);
        private UnityFileSystemProxy[] fileSystems = null;
        private bool HasFileSystems => fileSystems != null;

        private bool keepMeta = false;
        private bool deleteEmptyFolder = true;

        private void Initialize()
        {
            ChangePrevious(folderAsset);
            ChangePrevious(sourcePath);
        }

        private void ChangePrevious(DefaultAsset asset)
        {
            prev.SetFolderAsset(asset);
            SetPath(asset);
        }

        private void ChangePrevious(string sourcePath)
        {
            prev.SetSourcePath(sourcePath);
        }

        private void SetPath(DefaultAsset asset)
        {
            if (asset != null)
            {
                assetPath = AssetDatabase.GetAssetPath(asset);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    Debug.LogFormat("application data path : {0}\nassetPath : {1}\nresult : {2}",
                    Application.dataPath, assetPath.Substring(7), Path.Combine(Application.dataPath, assetPath.Substring(7)));
                    // assetPath의 맨 앞글자 'Assets/'를 지워야 하기 때문에 Substring 7
                    systemPath = Path.Combine(Application.dataPath, assetPath.Substring(7));
                }
            }
            else
            {
                assetPath = string.Empty;
                systemPath = string.Empty;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("대상", titleStyle);
            folderAsset = (DefaultAsset)EditorGUILayout.ObjectField("TargetFolder", folderAsset, typeof(DefaultAsset), true);
            if (prev.IsChangedFolderAsset(folderAsset))
            {
                ChangePrevious(folderAsset);
                Debug.LogFormat("IsChanged : {0}", assetPath);
            }
            if (HasTargetPath)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("에셋경로", GUILayout.Width(77));
                EditorGUILayout.SelectableLabel(assetPath, GUILayout.MinWidth(40), GUILayout.Height(18));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("시스템경로", GUILayout.Width(77));
                EditorGUILayout.SelectableLabel(systemPath, GUILayout.MinWidth(40), GUILayout.Height(18));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("원본", titleStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("경로", GUILayout.Width(77));
            sourcePath = EditorGUILayout.TextField(sourcePath);
            if (GUILayout.Button("로드", GUILayout.Width(77)))
                LoadUnityFileSystems(sourcePath);
            EditorGUILayout.EndHorizontal();
            if (prev.IsChangedSourcePath(sourcePath))
            {
                ChangePrevious(sourcePath);
                fileSystems = null;
            }
            if (HasSourcePath)
            {
                if (HasFileSystems)
                {
                    for (int i = 0; i < fileSystems.Length; i++)
                    {
                        fileSystems[i].selected = EditorGUILayout.ToggleLeft(fileSystems[i].name, fileSystems[i].selected);
                    }
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("전체 선택"))
                        SelectAllFileSystems(true);
                    if (GUILayout.Button("전체 해제"))
                        SelectAllFileSystems(false);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(5);
                    keepMeta = EditorGUILayout.ToggleLeft("meta 유지", keepMeta);
                    if (keepMeta)
                    {
                        deleteEmptyFolder = EditorGUILayout.ToggleLeft("empty 폴더 삭제", deleteEmptyFolder);
                    }
                    // if (GUILayout.Button("삭제 테스트"))
                    // {
                    //     RemoveSelectedFileSystem();
                    // }

                    if (GUILayout.Button("가져오기"))
                        Updating();
                }
            }
        }

        private void SelectAllFileSystems(bool selected)
        {
            for (int i = 0; i < fileSystems.Length; i++)
            {
                fileSystems[i].selected = selected;
            }
        }

        private void LoadUnityFileSystems(string sourcePath)
        {
            fileSystems = GetUnityFileSystems(sourcePath);
        }

        private UnityFileSystemProxy[] GetUnityFileSystems(string sourcePath)
        {
            string[] excludedLowerExtensions = new string[]
            {
                ".meta",
                ".ds_store"
            };


            DirectoryInfo root = new DirectoryInfo(sourcePath);

            DirectoryInfo[] directories = root.GetDirectories();
            FileInfo[] files = root.GetFiles();

            List<UnityFileSystemProxy> list = new List<UnityFileSystemProxy>();

            for (int i = 0; i < directories.Length; i++)
            {
                list.Add(new UnityDirectoryProxy(new UnityDirectory(directories[i])));
            }
            for (int i = 0; i < files.Length; i++)
            {
                string lowerExtension = files[i].Extension.ToLower();

                bool included = true;
                foreach (var extension in excludedLowerExtensions)
                {
                    if (lowerExtension == extension)
                    {
                        included = false;
                        break;
                    }
                }
                if (included)
                    list.Add(new UnityFileProxy(new UnityFile(files[i])));
            }

            if (list.Count > 0)
                return list.ToArray();
            else
                return null;
        }

        private void RemoveFileSystem(UnityFileSystemProxy unityFileSystem, bool keepMeta, bool deleteEmptyFolder)
        {
            if (unityFileSystem is IUnityDirectory)
            {
                RemoveUnityDirectory(unityFileSystem as IUnityDirectory, keepMeta, deleteEmptyFolder);
            }
            else if (unityFileSystem is IUnityFile)
            {
                RemoveUnityFile(unityFileSystem as IUnityFile, keepMeta);
            }
        }

        private void RemoveUnityDirectory(IUnityDirectory unityDirectory, bool keepMeta, bool deleteEmptyFolder)
        {
            if (keepMeta)
                unityDirectory.Drain(keepMeta: keepMeta, deleteEmptyFolder: deleteEmptyFolder);
            else
                unityDirectory.Delete();
        }
        private void RemoveUnityFile(IUnityFile unityFile, bool keepMeta)
        {
            unityFile.Delete(keepMeta);
        }

        private void Updating()
        {
            // 가져오기로한 
            for (int i = 0; i < fileSystems.Length; i++)
            {
                if (fileSystems[i].selected)
                {
                    Updating(fileSystems[i]);
                }
            }

            Complete();
        }

        private void Updating(IUnityFileSystem source)
        {
            if (source is IUnityFile)
            {
                Updating(source as IUnityFile, keepMeta);
            }
            else if (source is IUnityDirectory)
            {
                Updating(source as IUnityDirectory, keepMeta, deleteEmptyFolder);
            }
        }

        private void Updating(IUnityFile source, bool keepMeta)
        {
            string destinationPath = Path.Combine(systemPath, source.BaseInfo.Name);

            IUnityFile destination = new UnityFile(new FileInfo(destinationPath));

            BringFile(source.BaseInfo.FullName, destination.BaseInfo.FullName);

            if (!keepMeta || !destination.MetaFileInfo.Exists)
                BringFile(source.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"파일이 복사되었습니다: {source.BaseInfo.FullName} → {destinationPath}");
        }

        private void Updating(IUnityDirectory source, bool keepMeta, bool deleteEmptyFolder)
        {
            string destinationPath = Path.Combine(systemPath, source.BaseInfo.Name);

            IUnityDirectory destination = new UnityDirectory(new DirectoryInfo(destinationPath));
            if (destination.BaseInfo.Exists)
                RemoveUnityDirectory(destination, keepMeta, deleteEmptyFolder);
            BringDirectory(source.BaseInfo.FullName, destination.BaseInfo.FullName, keepMeta);

            if (!keepMeta || !destination.MetaFileInfo.Exists)
                BringFile(source.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"폴더가 복사되었습니다: {source.BaseInfo.FullName} → {destinationPath}");
        }

        private void Complete()
        {
            // 에셋 데이터베이스 새로 고침 (새로운 파일이나 변경된 파일을 감지)
            AssetDatabase.Refresh();

            // 스크립트 재컴파일 요청
            EditorUtility.RequestScriptReload();
        }

        private void BringFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Debug.LogWarning($"원본 파일이 존재하지 않습니다: {sourcePath}");
                return;
            }
            Debug.Log($"Copy source : {sourcePath}\ndestination : {destinationPath}");
            // 파일 덮어쓰기
            File.Copy(sourcePath, destinationPath, true);
            // IOException: Sharing violation on path /Users/daon/Desktop/SexyDu/PageViewSystem/MonoPageRoot.cs
            /// 이거 에러 뭐지?...;;
        }

        private void BringDirectory(string sourcePath, string destinationPath, bool excludedMeta = false)
        {
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"원본 폴더가 존재하지 않습니다: {sourcePath}");
                return;
            }

            // 대상 폴더가 없으면 생성
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // 원본 폴더 내의 모든 파일 복사
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                if (excludedMeta)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (extension == ".meta")
                        continue;
                }

                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationPath, fileName);

                // 파일 덮어쓰기
                File.Copy(file, destFile, true);
            }

            // 원본 폴더 내의 모든 하위 폴더 복사 (재귀적으로 처리)
            foreach (string dir in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(dir);
                string destDir = Path.Combine(destinationPath, dirName);

                // 하위 폴더를 재귀적으로 복사
                BringDirectory(dir, destDir, excludedMeta);
            }
        }

        public abstract class UnityFileSystemProxy : IUnityFileSystem
        {
            public bool selected = false;
            public string name
            {
                get;
                private set;
            }

            protected abstract UnityFileSystem FileSystem { get; }

            public FileSystemInfo BaseInfo => FileSystem.BaseInfo;

            public UnityFileSystemProxy(UnityFileSystem real)
            {
                name = real.BaseInfo.Name;
            }

            public virtual void Delete(bool keepMeta = false)
            {
                FileSystem.Delete(keepMeta);
            }

            public virtual FileInfo MetaFileInfo
            {
                get
                {
                    return FileSystem.MetaFileInfo;
                }

            }

            public virtual void DeleteMetaFile()
            {
                FileSystem.DeleteMetaFile();
            }
        }

        public class UnityFileProxy : UnityFileSystemProxy, IUnityFile
        {
            private readonly UnityFile real = null;

            protected override UnityFileSystem FileSystem => real;

            public UnityFileProxy(UnityFile real) : base(real)
            {
                this.real = real;
            }
        }

        public class UnityDirectoryProxy : UnityFileSystemProxy, IUnityDirectory
        {
            private readonly UnityDirectory real = null;

            protected override UnityFileSystem FileSystem => real;

            public UnityDirectoryProxy(UnityDirectory real) : base(real)
            {
                this.real = real;
            }

            public void Drain(bool keepMeta = false, bool deleteEmptyFolder = true)
            {
                real.Drain(keepMeta, deleteEmptyFolder);
            }
        }

        private struct ComparisonProperties
        {
            private DefaultAsset folderAsset;
            public ComparisonProperties SetFolderAsset(DefaultAsset folderAsset)
            {
                this.folderAsset = folderAsset;
                return this;
            }
            public bool IsChangedFolderAsset(DefaultAsset folderAsset)
            {
                return this.folderAsset != folderAsset;
            }

            private string sourcePath;
            public ComparisonProperties SetSourcePath(string sourcePath)
            {
                this.sourcePath = sourcePath;
                return this;
            }
            public bool IsChangedSourcePath(string sourcePath)
            {
                return this.sourcePath != sourcePath;
            }
        }
    }
}