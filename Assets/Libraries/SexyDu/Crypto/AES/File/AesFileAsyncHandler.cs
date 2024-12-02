using System.IO;
using System.Threading.Tasks;
using SexyDu.FileIO;

namespace SexyDu.Crypto
{
    /// <summary>
    /// 파일 AES 암호화 클래스 (비동기)
    /// </summary>
    public class AesFileAsyncHandler : AesBytes, IFileAsyncWriter, IFileAsyncReader
    {
        public AesFileAsyncHandler() : base() { }

        public AesFileAsyncHandler(byte[] key, byte[] iv) : base(key, iv) { }

        public AesFileAsyncHandler(char[] key, char[] iv) : base(key, iv) { }

        /// <summary>
        /// 파일 비동기 암호화 저장
        /// </summary>
        public async Task<byte[]> WriteAsync(string path, byte[] data)
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var encrypted = await EncryptAsync(data);

            await File.WriteAllBytesAsync(path, encrypted);

            return encrypted;
        }
        /// <summary>
        /// 파일 비동기 암호화 읽기
        /// </summary>
        public async Task<byte[]> ReadAsync(string path)
        {
            var data = await File.ReadAllBytesAsync(path);
            return await DecryptAsync(data);
        }
    }
}