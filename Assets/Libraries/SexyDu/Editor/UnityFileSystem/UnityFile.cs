using System.IO;

namespace SexyDu.OnEditor
{
    /// <summary>
    /// 유니티 파일 인터페이스
    /// </summary>
    public interface IUnityFile : IUnityFileSystem
    {

    }
    /// <summary>
    /// 유니티 파일 클래스
    /// </summary>
    public class UnityFile : UnityFileSystem, IUnityFile
    {
        /// <summary>
        /// 파일 정보
        /// </summary>
        private FileInfo info = null;
        /// <summary>
        /// 파일 시스템(파일 또는 폴더) 정보
        /// : UnityFileSystem
        /// </summary>
        public override FileSystemInfo BaseInfo => info;
        
        public UnityFile(string path) : this(new FileInfo(path))
        {
        }

        public UnityFile(FileInfo info)
        {
            this.info = info;
        }
    }
}