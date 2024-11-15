namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public class SelectableUnityFile : SelectableUnityFileSystem, IUnityFile
    {
        private readonly UnityFile real = null;

        protected override UnityFileSystem FileSystem => real;

        public SelectableUnityFile(UnityFile real) : base(real)
        {
            this.real = real;
        }
    }
}