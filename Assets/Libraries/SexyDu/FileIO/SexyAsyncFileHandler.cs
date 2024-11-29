using System;
using System.IO;
using System.Threading.Tasks;

namespace SexyDu.FileIO
{
    public abstract class SexyAsyncFileIO : IDisposable
    {
        public virtual void Dispose() { }
    }

    public class SexyAsyncFileReader : SexyAsyncFileIO, IFileAsyncReader
    {
        public async Task<byte[]> ReadAsync(string path)
        {
            return await File.ReadAllBytesAsync(path);
        }
    }

    public class SexyAsyncFileWriter : SexyAsyncFileIO, IFileAsyncWriter
    {
        public async Task<byte[]> WriteAsync(string path, byte[] data)
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            await File.WriteAllBytesAsync(path, data);

            return data;
        }
    }
}