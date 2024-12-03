using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using SexyDu.FileIO;
using UnityEngine;

namespace SexyDu.Network.Cache
{
    public abstract class BinaryCache : ICache
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
            if (worker != null)
            {
                worker.Dispose();
                worker = null;
            }
        }

        /// <summary>
        /// 작업 종료
        ///  * 로컬에서 작업 종료 시 호출
        /// </summary>
        protected virtual void Terminate()
        {
            Dispose();

#if UNITY_EDITOR
            UnityEngine.Debug.Log("BinaryCache Terminate");
#endif
        }

        /// <summary>
        /// 요청 수행 코루틴
        /// </summary>
        /// <param name="receipt">접수증</param>
        protected virtual IEnumerator CoRequest(ICacheReceipt receipt)
        {
            // uri 기반의 캐시 경로 가져오기
            string filePath = GetCachePath(receipt);

            // 캐시 파일이 존재하는 경우
            if (File.Exists(filePath))
            {
                // 캐시 파일 읽은 후 옵저버에 노티
                Task<byte[]> task = ReadFileAsync(filePath, receipt.encryptor);
                yield return new WaitUntil(() => task.IsCompleted);
                Notify(task.Result, 0, null, NetworkResult.Success);
            }
            // 캐시 파일이 존재하지 않는 경우
            else
            {
                byte[] responseData = null;

                // 다운로더 요청 후 캐시 파일 쓰기 및 옵저버에 노티
                INetworker worker = MakeDownloader().Request(receipt).Subscribe(res =>
                {
                    responseData = res.content;

                    // 옵저버에 노티
                    Notify(res.content, res.code, res.error, res.result);
                });
                // 작업 완료 대기
                yield return new WaitUntil(() => !worker.IsWorking);

                Task task = WriteFileAsync(filePath, responseData, receipt.encryptor);
                // 캐시 파일 쓰기
                yield return new WaitUntil(() => task.IsCompleted);
            }

            Terminate();
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
        protected IDisposable worker = null;
        /// <summary>
        /// 작업 중인지 여부
        /// </summary>
        public bool IsWorking => worker != null;

        /// <summary>
        /// 비동기 파일 읽기
        /// </summary>
        protected virtual async Task<byte[]> ReadFileAsync(string path, ICacheEncryptor encryptor)
        {
            using (var reader = MakeFileReader())
            {
                byte[] data = await reader.ReadAsync(path);
                if (encryptor != null)
                    return await encryptor.DecryptAsync(data);
                else
                    return data;
            }
        }
        /// <summary>
        /// 비동기 파일 쓰기
        /// </summary>
        protected virtual async Task WriteFileAsync(string path, byte[] data, ICacheEncryptor encryptor)
        {
            using (var writer = MakeFileWriter())
            {
                if (encryptor != null)
                    data = await encryptor.EncryptAsync(data);
                await writer.WriteAsync(path, data);
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
        /// <summary>
        /// Url에 따른 파일 경로 반환
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>파일 경로</returns>
        protected string GetCachePath(ICacheReceipt receipt)
        {
            return Path.Combine(CachePath, receipt.GetCacheFileName());
        }
        #endregion
    }
}