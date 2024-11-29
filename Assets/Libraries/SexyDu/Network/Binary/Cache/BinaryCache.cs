using System;
using System.Collections;
using System.IO;
using SexyDu.Crypto;
using SexyDu.FileIO;
using UnityEngine;

namespace SexyDu.Network
{
    public abstract class BinaryCache : ICache, IDisposable
    {
        // 기본 캐시 경로
        protected static readonly string BaseCachePath
#if UNITY_EDITOR
            = Path.Combine(Directory.GetCurrentDirectory(), "Cache"); // 유니티 에디터 환경의 경우 Assets 폴더 상위의 Cache 폴더 지정
#else
            = Path.Combine(UnityEngine.Application.persistentDataPath, "Cache"); // 유니티 빌드 환경의 경우 앱 내부 저장소의 Cache 폴더 지정
#endif
        // 특정 캐시 경로
        /// 해당 경로는 자식 클래스에서 재정의 가능
        protected virtual string CachePath => BaseCachePath;

        public virtual void Dispose()
        {
            if (loader != null)
            {
                loader.Dispose();
                loader = null;
            }
        }

        /// <summary>
        /// 옵저버에 노티
        /// </summary>
        /// <param name="res">bytes 수신 데이터</param>
        protected abstract void Notify(byte[] data, long code, string error, NetworkResult result);

        #region Load
        /// <summary>
        /// 작업중인 다운로더 Disposable
        /// </summary>
        protected IDisposable loader = null;
        /// <summary>
        /// 작업 중인지 여부
        /// </summary>
        public bool IsWorking => loader != null;

        /// <summary>
        /// 파일 읽고 옵저버에 노티하는 코루틴
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>자기 자신</returns>
        protected virtual IEnumerator CoReadFileAndNotify(string path)
        {
            using (var reader = MakeFileReader())
            {
                var task = reader.ReadAsync(path);
                yield return new WaitUntil(() => task.IsCompleted);
                Notify(task.Result, 200, null, NetworkResult.SuccessFromCache);
            }
        }
        /// <summary>
        /// 파일 쓰기 코루틴
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <param name="data">데이터</param>
        /// <returns>자기 자신</returns>
        protected virtual IEnumerator CoWriteFile(string path, byte[] data)
        {
            using (var writer = MakeFileWriter())
            {
                var task = writer.WriteAsync(path, data);
                yield return new WaitUntil(() => task.IsCompleted);
            }
        }

        /// <summary>
        /// 다운로더 반환
        /// </summary>
        /// <returns>다운로더</returns>
        protected virtual IBytesDownloader MakeDownloader()
        {
            return new SexyBytesDownloader();
        }
        /// <summary>
        /// 파일 리더 반환
        /// </summary>
        /// <returns>파일 리더</returns>
        protected virtual IFileAsyncReader MakeFileReader()
        {
            return new SexyAsyncFileReader();
        }
        /// <summary>
        /// 파일 라이터 반환
        /// </summary>
        /// <returns>파일 라이터</returns>
        protected virtual IFileAsyncWriter MakeFileWriter()
        {
            return new SexyAsyncFileWriter();
        }
        #endregion

        #region File
        // 파일명 암호화에 사용될 기본 Salt
        /// string : RvLMAT5gi+kmM4DnzJs5nA==
        /// EncryptionKeyGenerator (EditorWindow 'SexyDu/EncryptionKeyGenerator')에서 생성
        protected virtual char[] BaseHashSalt => new char[24] { 'R', 'v', 'L', 'M', 'A', 'T', '5', 'g', 'i', '+', 'k', 'm', 'M', '4', 'D', 'n', 'z', 'J', 's', '5', 'n', 'A', '=', '=' };

        /// <summary>
        /// Url에 따른 파일 경로 반환
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>파일 경로</returns>
        protected string GetCachePath(string url)
        {
            return Path.Combine(CachePath, GetCacheName(url));
        }

        /// <summary>
        /// Url에 따른 파일명 반환
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>파일명</returns>
        protected virtual string GetCacheName(string url)
        {
            using (SHA256Encryptor encryptor = new SHA256Encryptor())
            {
                return encryptor.Encrypt(url, BaseHashSalt);
            }
        }
        #endregion
    }
}