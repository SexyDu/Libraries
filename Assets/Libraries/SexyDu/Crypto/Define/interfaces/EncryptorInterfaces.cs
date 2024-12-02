/// 암화화에 사용될 인터페이스 모음
using System;
using System.Threading.Tasks;

namespace SexyDu.Crypto
{
    /// <summary>
    /// 바이트 암/복호화 인터페이스
    /// </summary>
    public interface IBytesEncryptor : IEncryptBytes, IDecryptBytes
    {
        // HMAC 사용 여부
        public bool UseHmac { get; }
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="base64key">base64 문자열 HMAC 키</param>
        public IBytesEncryptor SetHmac(char[] base64key);
        /// <summary>
        /// HMAC 설정
        /// </summary>
        /// <param name="key">HMAC 키</param>
        public IBytesEncryptor SetHmac(byte[] key);
    }

    /// <summary>
    /// 바이트 암호화 인터페이스
    /// </summary>
    public interface IEncryptBytes : IDisposable
    {
        byte[] Encrypt(byte[] bytes);
        Task<byte[]> EncryptAsync(byte[] bytes);
    }
    /// <summary>
    /// 바이트 복호화 인터페이스
    /// </summary>
    public interface IDecryptBytes : IDisposable
    {
        byte[] Decrypt(byte[] bytes);
        Task<byte[]> DecryptAsync(byte[] bytes);
    }

    /// <summary>
    /// 문자열 암/복호화 인터페이스
    /// </summary>
    public interface IStringEncryptor : IEncryptString, IDecryptString { }

    /// <summary>
    /// 문자열 암호화 인터페이스
    /// </summary>
    public interface IEncryptString : IDisposable
    {
        string Encrypt(string str);
    }

    /// <summary>
    /// 문자열 복호화 인터페이스
    /// </summary>
    public interface IDecryptString : IDisposable
    {
        string Decrypt(string str);
    }
}