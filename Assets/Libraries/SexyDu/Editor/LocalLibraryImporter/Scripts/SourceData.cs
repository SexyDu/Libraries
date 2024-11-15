using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public struct SourceData
    {
        // 원본 폴더 경로
        public string path;
        // public bool HasSourcePath => !string.IsNullOrEmpty(path);
        public SelectableUnityFileSystem[] fileSystems
        {
            get;
            private set;
        }
        public bool HasFileSystems => fileSystems != null;

        /// <summary>
        /// 현재 설정 경로의 파일 시스템 로드
        /// </summary>
        public void LoadUnityFileSystems()
        {
            fileSystems = GetUnityFileSystems(path);
        }
        /// <summary>
        /// 특정 경로의 파일 시스템 배열 반환
        /// </summary>
        private SelectableUnityFileSystem[] GetUnityFileSystems(string path)
        {
            string[] excludedLowerExtensions = new string[]
            {
                ".meta",
                ".ds_store"
            };

            DirectoryInfo root = new DirectoryInfo(path);

            DirectoryInfo[] directories = root.GetDirectories();
            FileInfo[] files = root.GetFiles();

            List<SelectableUnityFileSystem> list = new List<SelectableUnityFileSystem>();

            for (int i = 0; i < directories.Length; i++)
            {
                list.Add(new SelectableUnityDirectory(new UnityDirectory(directories[i])));
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
                    list.Add(new SelectableUnityFile(new UnityFile(files[i])));
            }

            if (list.Count > 0)
                return list.ToArray();
            else
                return null;
        }

        /// <summary>
        /// 파일 시스템 클리어
        /// </summary>
        public void ClearFileSystems()
        {
            fileSystems = null;
        }
        /// <summary>
        /// 선택된 파일 시스템 인터페이스 반환
        /// </summary>
        public IUnityFileSystem[] GetSelectedFileSystems()
        {
            List<IUnityFileSystem> list = new List<IUnityFileSystem>();

            for (int i = 0; i < fileSystems.Length; i++)
            {
                if (fileSystems[i].selected)
                    list.Add(fileSystems[i]);
            }

            return list.ToArray();
        }
        /// <summary>
        /// 전체 선택 상태 설정
        /// </summary>
        public void SelectAll(bool selected)
        {
            for (int i = 0; i < fileSystems.Length; i++)
            {
                fileSystems[i].selected = selected;
            }
        }

        /// 값 또는 오브젝트 연결 변경 확인 기능
        #region Comparison
        private string cPath;

        public bool UnsyncedPath => path != cPath;

        public void SyncronizePath()
        {
            cPath = path;
        }
        #endregion

    }
}