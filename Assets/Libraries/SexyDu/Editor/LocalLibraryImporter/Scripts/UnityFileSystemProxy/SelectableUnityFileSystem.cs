using System.IO;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public abstract class SelectableUnityFileSystem : IUnityFileSystem
    {
        public bool selected = false;
        public string name
        {
            get;
            private set;
        }

        protected abstract UnityFileSystem FileSystem { get; }

        public FileSystemInfo BaseInfo => FileSystem.BaseInfo;

        public SelectableUnityFileSystem(UnityFileSystem real)
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
}