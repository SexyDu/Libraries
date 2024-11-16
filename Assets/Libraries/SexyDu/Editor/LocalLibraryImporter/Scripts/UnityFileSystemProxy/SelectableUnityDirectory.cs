namespace SexyDu.OnEditor.LocalLibraryImporter
{
    /// <summary>
    /// 선택형 유니티 폴더 프록시(패턴)
    /// </summary>
    public class SelectableUnityDirectory : SelectableUnityFileSystem, IUnityDirectory
    {
        // 실체
        private readonly UnityDirectory real = null;

        // 실체 유니티 파일 시스템
        protected override UnityFileSystem FileSystem => real;

        public SelectableUnityDirectory(UnityDirectory real) : base(real)
        {
            this.real = real;
        }

        #region IUnityDirectory Proxy 정의
        public void Drain(bool keepMeta = false, bool deleteEmptyFolder = true)
        {
            real.Drain(keepMeta, deleteEmptyFolder);
        }
        #endregion
    }
}