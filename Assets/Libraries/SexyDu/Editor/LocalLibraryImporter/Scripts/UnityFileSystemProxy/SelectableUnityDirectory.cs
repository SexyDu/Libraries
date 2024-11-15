namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public class SelectableUnityDirectory : SelectableUnityFileSystem, IUnityDirectory
    {
        private readonly UnityDirectory real = null;

        protected override UnityFileSystem FileSystem => real;

        public SelectableUnityDirectory(UnityDirectory real) : base(real)
        {
            this.real = real;
        }

        public void Drain(bool keepMeta = false, bool deleteEmptyFolder = true)
        {
            real.Drain(keepMeta, deleteEmptyFolder);
        }
    }
}