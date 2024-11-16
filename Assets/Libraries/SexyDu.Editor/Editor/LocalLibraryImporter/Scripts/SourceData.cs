using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 원본 폴더 Scriptable Object
    /// * Scriptable Object로 한 이유는 다시 윈도우를 활성화 할 떄 이 전에 사용했던 정보를 유지하기 위함.
    /// </summary>
    [CreateAssetMenu(fileName = "SourceData", menuName = "SexyDu/Editor/LocalLibraryImporter/SourceData")]
    public class SourceData : ScriptableObject
    {
        public const string ResourcePath = "LocalLibraryImporter/SourceData";

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
            // 제외할 파일의 확장자 (소문자 기준)
            string[] excludedLowerExtensions = new string[]
            {
                ".meta", // 메타파일, 이는 UnityFileSystem에 의해 제어되기 때문에 여기에선 제외됨
                ".ds_store" // MacOS가 만들어 내는 쓰레기 파일
            };
            // 경로의 폴더 정보 가져오기
            DirectoryInfo root = new DirectoryInfo(path);

            if (!root.Exists)
            {
                EditorUtility.DisplayDialog("경고", "원본 경로가 존재하지 않습니다.", "OK");
                return null;
            }

            // 폴더 및 파일 정보 가져오기
            DirectoryInfo[] directories = root.GetDirectories();
            FileInfo[] files = root.GetFiles();

            // 선택가능 파일 시스템(프록시) 리스트 생성
            List<SelectableUnityFileSystem> list = new List<SelectableUnityFileSystem>();

            // UnityDirectory 구성
            for (int i = 0; i < directories.Length; i++)
            {
                list.Add(new SelectableUnityDirectory(new UnityDirectory(directories[i])));
            }
            // UnityFile 구성
            for (int i = 0; i < files.Length; i++)
            {
                // 확장자(소문자) 가져오기
                string lowerExtension = files[i].Extension.ToLower();
#if true
                // 제외 확장자에 포함되지 않은 경우만 리스트에 추가
                if (!excludedLowerExtensions.Contains(lowerExtension))
                    list.Add(new SelectableUnityFile(new UnityFile(files[i])));
#else
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
#endif
            }

            // 리스트에 데이터가 있는 경우 배열로 반환
            if (list.Count > 0)
                return list.ToArray();
            // 아닌 경우 null 반환
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
        private string cPath; // 원본 경로 비교 객체
        // 원 객체와 비교객체 비동기 여부
        public bool UnsyncedPath => path != cPath;
        /// <summary>
        /// 원 객체와 비교 객체 동기화
        /// </summary>
        public void SyncronizePath()
        {
            cPath = path;

            EditorUtility.SetDirty(this);
        }
        #endregion

    }
}