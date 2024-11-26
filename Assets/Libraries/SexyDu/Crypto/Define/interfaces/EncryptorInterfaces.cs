/// 암화화에 사용될 인터페이스 모음

using System;

namespace SexyDu.Crypto
{
    /// <summary>
    /// 바이트 암호화 인터페이스
    /// </summary>
    public interface IEncryptBytes : IDisposable
    {
        byte[] Encrypt(byte[] bytes);
    }

    /// <summary>
    /// 바이트 복호화 인터페이스
    /// </summary>
    public interface IDecryptBytes : IDisposable
    {
        byte[] Decrypt(byte[] bytes);
    }

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

    /// <summary>
    /// 파일 암호화 인터페이스
    /// </summary>
    public interface IEncryptedFileWriter : IDisposable
    {
        void Write(string path, byte[] data);
    }

    /// <summary>
    /// 파일 복호화 인터페이스
    /// </summary>
    public interface IEncryptedFileReader : IDisposable
    {
        byte[] Read(string path);
    }
}