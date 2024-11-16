using System.IO;
using UnityEditor;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 대상 폴더 Scriptable Object
    /// * Scriptable Object로 한 이유는 다시 윈도우를 활성화 할 떄 이 전에 사용했던 정보를 유지하기 위함.
    /// </summary>
    [CreateAssetMenu(fileName = "TargetFolder", menuName = "SexyDu/Editor/LocalLibraryImporter/TargetFolder")]
    public partial class TargetFolder : ScriptableObject
    {
        public const string ResourcePath = "LocalLibraryImporter/TargetFolder";

        // 가져올 대상 폴더 에셋
        public DefaultAsset folderAsset;
        // 에셋 경로
        public string assetPath
        {
            get;
            private set;
        }
        // 에셋의 시스템 경로
        public string systemPath
        {
            get;
            private set;
        }
        // 대상 폴더 에셋이 설정되었는지 여부
        public bool HasTargetFolder => folderAsset != null;
        // 대상 에셋 경로가 설정되었는지 여부
        public bool HasTargetPath => !string.IsNullOrEmpty(assetPath);

        /// <summary>
        /// 대상 폴더 에셋에 대한 경로 설정
        /// </summary>
        public void SetPath()
        {
            SetPath(folderAsset);
        }

        /// <summary>
        /// 대상 폴더 에셋에 대한 경로 설정
        /// </summary>
        private void SetPath(DefaultAsset asset)
        {
            if (asset != null)
            {
                assetPath = AssetDatabase.GetAssetPath(asset);
                if (!string.IsNullOrEmpty(assetPath))
                {
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

        // 가져올 경우 기존 에셋의 meta 파일을 유지할지 여부
        public bool keepMeta;
        // (meta 파일을 유지할 경우) 폴더 비우기 시 비어있는 폴더를 삭제할 지 여부
        public bool deleteEmptyFolder;

        /// <summary>
        /// 여려개의 파일 시스템(파일 또는 폴더) 가져오기
        /// </summary>
        public void Bring(IUnityFileSystem[] fileSystems)
        {
            for (int i = 0; i < fileSystems.Length; i++)
            {
                Bring(fileSystems[i]);
            }
        }
        /// <summary>
        /// 파일 시스템(파일 또는 폴더) 가져오기
        /// </summary>
        private void Bring(IUnityFileSystem source)
        {
            if (source is IUnityFile)
                Bring(source as IUnityFile);
            else if (source is IUnityDirectory)
                Bring(source as IUnityDirectory);
        }
        /// <summary>
        /// 폴더 가져오기
        /// </summary>
        private void Bring(IUnityDirectory source)
        {
            // 목표 경로 설정
            string destinationPath = Path.Combine(systemPath, source.BaseInfo.Name);
            // 목표에 대한 UnityDirectory 생성
            IUnityDirectory destination = new UnityDirectory(new DirectoryInfo(destinationPath));
            // 이미 존재하는 경우 제거
            if (destination.BaseInfo.Exists)
                RemoveUnityDirectory(destination);
            // 소스의 폴더를 목표 폴더로 복사
            CopyDirectory(source.BaseInfo.FullName, destination.BaseInfo.FullName, keepMeta);

            // 메타를 유지하지 않거나 목표위치에 메타파일이 없는 경우 해당 폴더의 메타까지 가져오기
            if (!keepMeta || !destination.MetaFileInfo.Exists)
                CopyFile(source.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"폴더가 복사되었습니다: {source.BaseInfo.FullName} → {destinationPath}");
        }
        /// <summary>
        /// 파일 가져오기
        /// </summary>
        private void Bring(IUnityFile source)
        {
            // 목표 경로 설정
            string destinationPath = Path.Combine(systemPath, source.BaseInfo.Name);
            // 목표에 대한 UnityFile todtjd
            IUnityFile destination = new UnityFile(new FileInfo(destinationPath));
            // 소스의 파일을 목표 위치에 복사
            CopyFile(source.BaseInfo.FullName, destination.BaseInfo.FullName);

            // 메타를 유지하지 않거나 목표위치에 메타파일이 없는 경우 해당 폴더의 메타까지 가져오기
            if (!keepMeta || !destination.MetaFileInfo.Exists)
                CopyFile(source.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"파일이 복사되었습니다: {source.BaseInfo.FullName} → {destinationPath}");
        }

        /// <summary>
        /// 전달받은 폴더 삭제 또는 비우기
        /// </summary>
        /// <param name="unityDirectory"></param>
        private void RemoveUnityDirectory(IUnityDirectory unityDirectory)
        {
            // 메타가 유지되는 경우 비우기
            if (keepMeta)
                unityDirectory.Drain(keepMeta: keepMeta, deleteEmptyFolder: deleteEmptyFolder);
            // 그렇지 않으면 바로 삭제 해버리기
            else
                unityDirectory.Delete();
        }

        /// <summary>
        /// 소스 경로의 파일을 목표 경로로 복사
        /// </summary>
        /// <param name="sourcePath">소스 경로</param>
        /// <param name="destinationPath">목표 경로</param>
        private void CopyFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Debug.LogWarning($"원본 파일이 존재하지 않습니다: {sourcePath}");
                return;
            }

            // 파일 덮어쓰기
            File.Copy(sourcePath, destinationPath, true);
        }
        /// <summary>
        /// 소스 경로의 폴더를 목표 경로로 복사
        /// </summary>
        /// <param name="sourcePath">소스 경로</param>
        /// <param name="destinationPath">목표 경로</param>
        /// <param name="excludedMeta">메타파일 제외 여부</param>
        private void CopyDirectory(string sourcePath, string destinationPath, bool excludedMeta = false)
        {
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogWarning($"원본 폴더가 존재하지 않습니다: {sourcePath}");
                return;
            }

            // 대상 폴더가 없으면 생성
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            // 원본 폴더 내의 모든 파일 복사
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                // 메타 제외 상태인 경우 메타 거르기
                if (excludedMeta)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (extension == ".meta")
                        continue;
                }
                // 파일 이름 가져오기
                string fileName = Path.GetFileName(file);
                // 목표 파일 경로 설정
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
                CopyDirectory(dir, destDir, excludedMeta);
            }
        }

        /// 값 또는 오브젝트 연결 변경 확인 기능
        #region Comparison
        private DefaultAsset cFolderAsset; // 폴더에셋 비교 객체
        // 원 객체와 비교객체 비동기 여부
        public bool UnsyncedFolderAsset => folderAsset != cFolderAsset;
        /// <summary>
        /// 원 객체와 비교 객체 동기화
        /// </summary>
        public void SyncronizeFolderAsset()
        {
            cFolderAsset = folderAsset;

            EditorUtility.SetDirty(this);
        }
        #endregion
    }
}