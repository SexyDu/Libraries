using System.IO;
using UnityEditor;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public partial class TargetFolder
    {
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
        /// 파일 시스템(파일 또는 폴더) 가져오기
        /// </summary>
        /// <param name="fileSystems"></param>
        public void Bring(IUnityFileSystem[] fileSystems)
        {
            for (int i = 0; i < fileSystems.Length; i++)
            {
                Bring(fileSystems[i]);
            }
        }

        private void Bring(IUnityFileSystem fileSystem)
        {
            if (fileSystem is IUnityFile)
                Bring(fileSystem as IUnityFile);
            else if (fileSystem is IUnityDirectory)
                Bring(fileSystem as IUnityDirectory);
        }

        private void Bring(IUnityDirectory directory)
        {
            string destinationPath = Path.Combine(systemPath, directory.BaseInfo.Name);

            IUnityDirectory destination = new UnityDirectory(new DirectoryInfo(destinationPath));
            if (destination.BaseInfo.Exists)
                RemoveUnityDirectory(destination);
            CopyDirectory(directory.BaseInfo.FullName, destination.BaseInfo.FullName, keepMeta);

            if (!keepMeta || !destination.MetaFileInfo.Exists)
                CopyFile(directory.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"폴더가 복사되었습니다: {directory.BaseInfo.FullName} → {destinationPath}");
        }

        private void Bring(IUnityFile file)
        {
            string destinationPath = Path.Combine(systemPath, file.BaseInfo.Name);

            IUnityFile destination = new UnityFile(new FileInfo(destinationPath));

            CopyFile(file.BaseInfo.FullName, destination.BaseInfo.FullName);

            if (!keepMeta || !destination.MetaFileInfo.Exists)
                CopyFile(file.MetaFileInfo.FullName, destination.MetaFileInfo.FullName);

            Debug.Log($"파일이 복사되었습니다: {file.BaseInfo.FullName} → {destinationPath}");
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

        private void CopyDirectory(string sourcePath, string destinationPath, bool excludedMeta = false)
        {
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogWarning($"원본 폴더가 존재하지 않습니다: {sourcePath}");
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
                CopyDirectory(dir, destDir, excludedMeta);
            }
        }

        /// 값 또는 오브젝트 연결 변경 확인 기능
        #region Comparison
        private DefaultAsset cFolderAsset;

        public bool UnsyncedFolderAsset => folderAsset != cFolderAsset;

        public void SyncronizeFolderAsset()
        {
            cFolderAsset = folderAsset;
        }
        #endregion
    }
}