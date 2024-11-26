using System.IO;

namespace SexyDu.Crypto
{
    /// <summary>
    /// 파일 AES 암호화 클래스
    /// </summary>
    public class AesFile : AesBytes, IEncryptedFileWriter, IEncryptedFileReader
    {
        public AesFile() : base() { }

        public AesFile(byte[] key, byte[] iv) : base(key, iv) { }

        /// <summary>
        /// 파일 암호화하여 저장
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <param name="data">데이터</param>
        public void Write(string path, byte[] data)
        {
            File.WriteAllBytes(path, Encrypt(data));
        }
        /// <summary>
        /// 파일 복호화하여 읽기
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>복호화 데이터</returns>
        public byte[] Read(string path)
        {
            return Decrypt(File.ReadAllBytes(path));
        }
    }
}