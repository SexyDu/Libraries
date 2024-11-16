using System.IO;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 선택형 유니티 파일 시스템 프록시(패턴)
    /// </summary>
    public abstract class SelectableUnityFileSystem : IUnityFileSystem
    {
        // 파일시스템 선택 여부
        public bool selected = false;
        // 파일 이름
        public string name
        {
            get;
            private set;
        }
        // 실체 유니티 파일 시스템
        protected abstract UnityFileSystem FileSystem { get; }

        public SelectableUnityFileSystem(UnityFileSystem real)
        {
            name = real.BaseInfo.Name;
        }

        #region IUnityFileSystem Proxy 정의
        public FileSystemInfo BaseInfo => FileSystem.BaseInfo;

        public virtual FileInfo MetaFileInfo => FileSystem.MetaFileInfo;

        public virtual void Delete(bool keepMeta = false)
        {
            FileSystem.Delete(keepMeta);
        }

        public virtual void DeleteMetaFile()
        {
            FileSystem.DeleteMetaFile();
        }
        #endregion

    }
}