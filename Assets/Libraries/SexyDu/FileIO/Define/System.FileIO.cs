using System;
using System.Threading.Tasks;

namespace SexyDu
{
    namespace FileIO
    {
        /// <summary>
        /// 비동기 파일 쓰기
        /// </summary>
        public interface IFileAsyncWriter : IDisposable
        {
            public Task<byte[]> WriteAsync(string path, byte[] data);
        }
        /// <summary>
        /// 비동기 파일 읽기
        /// </summary>
        public interface IFileAsyncReader : IDisposable
        {
            public Task<byte[]> ReadAsync(string path);
        }
        /// <summary>
        /// 파일 쓰기
        /// </summary>
        public interface IFileWriter
        {
            public void Write(string path, byte[] data);
        }
        /// <summary>
        /// 파일 읽기
        /// </summary>
        public interface IFileReader
        {
            public byte[] Read(string path);
        }
    }
}