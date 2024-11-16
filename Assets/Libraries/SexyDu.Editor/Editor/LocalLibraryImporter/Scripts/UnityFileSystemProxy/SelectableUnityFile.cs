namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 선택형 유니티 파일 프록시(패턴)
    /// </summary>
    public class SelectableUnityFile : SelectableUnityFileSystem, IUnityFile
    {
        // 실체
        private readonly UnityFile real = null;

        // 실체 유니티 파일 시스템
        protected override UnityFileSystem FileSystem => real;

        public SelectableUnityFile(UnityFile real) : base(real)
        {
            this.real = real;
        }
    }
}