using System;
using System.IO;
using System.Security.Cryptography;

namespace SexyDu.Crypto
{
    . // 아래 코드 정리하고 주석 달자
    public class AesEncryptor
    {
        private static readonly char[] HMAC_Chars = new char[44] { 'E', 'U', 'J', '3', 'K', 'Q', 'x', 'b', 'H', 'Y', 'd', 'Q', 'f', 'z', 'D', 'd', 'j', 'n', 'd', '2', 'P', 'p', 'b', 'X', 'V', 'f', 'Q', 'D', '3', 'Z', '7', 'G', 'u', 'g', 'B', '8', 'C', 'd', '/', 'I', 'g', 'R', 'A', '=' };
        private readonly byte[] HMAC_KEY = Convert.FromBase64CharArray(HMAC_Chars, 0, HMAC_Chars.Length);

        private byte[] GetHmacHash(byte[] data)
        {
            using (HMACSHA256 hmac = new HMACSHA256(HMAC_KEY))
                return hmac.ComputeHash(data);
        }

        private byte[] ApplyHmac(byte[] data)
        {
            return Combine(data, GetHmacHash(data));
        }

        private byte[] SkimHmac(byte[] data)
        {
            byte[] encrypted = new byte[data.Length - HMAC_KEY.Length];
            byte[] hmacHash = new byte[HMAC_KEY.Length];
            Buffer.BlockCopy(data, 0, encrypted, 0, encrypted.Length);
            Buffer.BlockCopy(data, encrypted.Length, hmacHash, 0, hmacHash.Length);

            using (HMACSHA256 hmac = new HMACSHA256(HMAC_KEY))
            {
                byte[] computedHash = hmac.ComputeHash(encrypted);
                if (!Compare(computedHash, hmacHash))
                    throw new HmacVerificationException("HMAC 검증 실패: 데이터가 변조되었을 수 있습니다.");
            }
            return encrypted;
        }

        private byte[] Combine(byte[] data1, byte[] data2)
        {
            byte[] result = new byte[data1.Length + data2.Length];
            Buffer.BlockCopy(data1, 0, result, 0, data1.Length);
            Buffer.BlockCopy(data2, 0, result, data1.Length, data2.Length);
            return result;
        }

        private bool Compare(byte[] data1, byte[] data2)
        {
            if (data1.Length != data2.Length) return false;
            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] != data2[i]) return false;
            }
            return true;
        }

        public void Encrypt(string inputFile, string outputFile, byte[] key, byte[] iv, bool hmac = false)
        {
            byte[] data = File.ReadAllBytes(inputFile);
            byte[] result = Encrypt(data, key, iv);

            if (hmac)
                result = ApplyHmac(result);

            File.WriteAllBytes(outputFile, result);
        }

        public byte[] Decrypt(string targetFile, byte[] key, byte[] iv, bool hmac = false)
        {
            byte[] data = File.ReadAllBytes(targetFile);
            if (hmac)
                data = SkimHmac(data);

            return Decrypt(data, key, iv);
        }

        private MemoryStream MakeEncryptStream(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                UnityEngine.Debug.LogFormat("Mode : {0}, Padding : {1}, BlockSize : {2}", aes.Mode, aes.Padding, aes.BlockSize);

                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
            }
            return ms;
        }

        private byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            byte[] result = null;
            using (MemoryStream ms = MakeEncryptStream(data, key, iv))
            {
                result = ms.ToArray();
            }
            return result;
        }

        private MemoryStream MakeDecryptStream(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                UnityEngine.Debug.LogFormat("Mode : {0}, Padding : {1}, BlockSize : {2}", aes.Mode, aes.Padding, aes.BlockSize);

                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
            }
            return ms;
        }

        private byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (MemoryStream ms = MakeDecryptStream(data, key, iv))
            {
                return ms.ToArray();
            }
        }
    }
}
