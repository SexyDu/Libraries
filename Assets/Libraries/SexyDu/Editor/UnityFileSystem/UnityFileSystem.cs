using System;
using System.IO;

namespace SexyDu.OnEditor
{
    /// <summary>
    /// 유니티 파일 시스템 인터페이스
    /// * 유니티 파일 시스템이라 함은 유니티의 'Assets/' 폴더 내부에 있는 파일들을 의미함
    /// * 즉, '.meta' 파일을 가지는 모든 파일을 의미
    /// </summary>
    public interface IUnityFileSystem
    {
        /// <summary>
        /// 파일 시스템(파일 또는 폴더) 정보
        /// </summary>
        public FileSystemInfo BaseInfo { get; }
        /// <summary>
        /// 해당 파일 시스템 삭제
        /// </summary>
        /// <param name="keepMeta">.meta 파일 유지 여부</param>
        public void Delete(bool keepMeta = false);
        /// <summary>
        /// .meta 파일 정보 반환
        /// </summary>
        public FileInfo MetaFileInfo { get; }
        /// <summary>
        /// .meta 파일 삭제
        /// </summary>
        public void DeleteMetaFile();
    }
    /// <summary>
    /// 유니티 파일 시스템 기본 클래스
    /// </summary>
    public abstract class UnityFileSystem : IUnityFileSystem
    {
        /// <summary>
        /// 파일 시스템(파일 또는 폴더) 정보
        /// : IUnityFileSystem
        /// </summary>
        public abstract FileSystemInfo BaseInfo { get; }
        // 메타파일 확장자
        protected const string MetaExtensionLower = ".meta";
        /// <summary>
        /// 해당 파일 시스템 삭제
        /// : IUnityFileSystem
        /// </summary>
        /// <param name="keepMeta">.meta 파일 유지 여부</param>
        public virtual void Delete(bool keepMeta = false)
        {
            try
            {
                if (BaseInfo.Exists)
                {
                    BaseInfo.Delete();

                    if (!keepMeta)
                        DeleteMetaFile();
                }
            }
            catch (IOException e)
            {
                UnityEngine.Debug.LogFormat("'{0}' 지우다가 에러", BaseInfo.Name);
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// .meta 파일 정보 반환
        /// : IUnityFileSystem
        /// </summary>
        public virtual FileInfo MetaFileInfo
        {
            get
            {
                string metaFilePath = string.Format("{0}.meta", BaseInfo.FullName);

                return new FileInfo(metaFilePath);
            }
        }
        /// <summary>
        /// .meta 파일 삭제
        /// : IUnityFileSystem
        /// </summary>
        public virtual void DeleteMetaFile()
        {
            FileInfo metaFileInfo = MetaFileInfo;
            if (metaFileInfo.Exists)
                metaFileInfo.Delete();
        }

    }
}