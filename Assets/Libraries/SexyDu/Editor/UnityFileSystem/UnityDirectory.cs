using System.IO;

namespace SexyDu.OnEditor
{
    /// <summary>
    /// 유니티 폴더 인터페이스
    /// </summary>
    public interface IUnityDirectory : IUnityFileSystem
    {
        /// <summary>
        /// 폴더 내부 비우기
        /// </summary>
        /// <param name="keepMeta">메타파일 유지 여부</param>
        /// <param name="deleteEmptyFolder">(메타파일 유지에 따른 고려) 실행 후 폴더 내부가 비어있는 경우 해당 폴더 삭제 여부</param>
        public void Drain(bool keepMeta = false, bool deleteEmptyFolder = true);
    }
    /// <summary>
    /// 유니티 폴더 클래스
    /// </summary>
    public class UnityDirectory : UnityFileSystem, IUnityDirectory
    {
        /// <summary>
        /// 폴더 정보
        /// </summary>
        private DirectoryInfo info = null;
        /// <summary>
        /// 파일 시스템(파일 또는 폴더) 정보
        /// : UnityFileSystem
        /// </summary>
        public override FileSystemInfo BaseInfo => info;

        public UnityDirectory(string path) : this(new DirectoryInfo(path))
        {

        }

        public UnityDirectory(DirectoryInfo info)
        {
            this.info = info;
        }
        /// <summary>
        /// 해당 파일 시스템 삭제
        /// : UnityFileSystem
        /// </summary>
        /// <param name="keepMeta">.meta 파일 유지 여부</param>
        public override void Delete(bool keepMeta = false)
        {
            if (info.Exists)
            {
                info.Delete(true);

                if (!keepMeta)
                    DeleteMetaFile();
            }
        }
        /// <summary>
        /// 폴더 내부 비우기
        /// </summary>
        /// <param name="keepMeta">메타파일 유지 여부</param>
        /// <param name="deleteEmptyFolder">(메타파일 유지에 따른 고려) 실행 후 폴더 내부가 비어있는 경우 해당 폴더 삭제 여부</param>
        public void Drain(bool keepMeta = false, bool deleteEmptyFolder = true)
        {
            // 전체 파일 가져오기
            FileInfo[] files = info.GetFiles();
            // 전체 파일 삭제
            for (int i = 0; i < files.Length; i++)
            {
                // 메타파일을 제외한 파일을 UnityFile로 가져와서 삭제
                /// 이리 번거롭게 하는 이유는 메타파일 유지에 대한 고려 때문
                /// 이 또한 굳이인 느낌이지만 코드 통일성 때문에 이리함.
                /// 에디터 코드라 성능에 대한 문제도 크게 상관 없음
                /// 그리고 그냥 FileInfo를 통해 바로 삭제할 경우 Directory의 .meta 파일을 거르는게 더 애매하기 때문
                /// 암튼 이유 있음
                if (files[i].Extension.ToLower() != MetaExtensionLower)
                    new UnityFile(files[i]).Delete(keepMeta);
            }

            // 전체 폴더 가져오기
            DirectoryInfo[] directories = info.GetDirectories();
            // 하위 전체 폴더 비우기
            for (int i = 0; i < directories.Length; i++)
            {
                // 유니티 폴더 가져와서 비우기
                UnityDirectory unityDirectory = new UnityDirectory(directories[i]);
                unityDirectory.Drain(keepMeta, deleteEmptyFolder);
            }
            // 만일 폴더가 전부 비워져 있는 경우
            if (deleteEmptyFolder)
            {
                // 남이 있는 파일 시스템 가져오기
                FileSystemInfo[] remains = info.GetFileSystemInfos();
                // 없는 경우 삭제
                if (remains.Length == 0)
                    Delete();
            }
        }
    }
}