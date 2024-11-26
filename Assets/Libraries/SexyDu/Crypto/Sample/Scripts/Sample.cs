using System.IO;
using UnityEngine;

namespace SexyDu.Crypto
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] private string inputFile;
        [SerializeField] private string outputFile;
        [SerializeField] private Texture2D texture2D;

        // 2PqHQ/DxV5QxMM9RH6fh7Z7pSSNbqsauwQVG25d4IyU=
        private readonly char[] caKey = new char[44] { '2', 'P', 'q', 'H', 'Q', '/', 'D', 'x', 'V', '5', 'Q', 'x', 'M', 'M', '9', 'R', 'H', '6', 'f', 'h', '7', 'Z', '7', 'p', 'S', 'S', 'N', 'b', 'q', 's', 'a', 'u', 'w', 'Q', 'V', 'G', '2', '5', 'd', '4', 'I', 'y', 'U', '=' };
        // lR3ynui08Gqq4gmkA3osNA==
        private readonly char[] caIv = new char[24] { 'l', 'R', '3', 'y', 'n', 'u', 'i', '0', '8', 'G', 'q', 'q', '4', 'g', 'm', 'k', 'A', '3', 'o', 's', 'N', 'A', '=', '=' };
        private readonly char[] HMAC_Chars = new char[44] { 'E', 'U', 'J', '3', 'K', 'Q', 'x', 'b', 'H', 'Y', 'd', 'Q', 'f', 'z', 'D', 'd', 'j', 'n', 'd', '2', 'P', 'p', 'b', 'X', 'V', 'f', 'Q', 'D', '3', 'Z', '7', 'G', 'u', 'g', 'B', '8', 'C', 'd', '/', 'I', 'g', 'R', 'A', '=' };

        private void Encrypt()
        {
            using (AesFile aes = new AesFile())
            {
                aes.SetHmac(HMAC_Chars).SetKey(caKey).SetIv(caIv);

                aes.Write(outputFile, File.ReadAllBytes(inputFile));
            }
        }

        private void Decrypt()
        {
            using (AesFile aes = new AesFile())
            {
                aes.SetHmac(HMAC_Chars).SetKey(caKey).SetIv(caIv);

                texture2D = ByteArrayToTexture2D(aes.Read(outputFile));
            }
        }

        [SerializeField] private string plainText;
        [SerializeField] private string salt;
        [SerializeField] private int iteration = 1;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 100), "Encrypt"))
            {
                Encrypt();
            }

            if (GUI.Button(new Rect(110, 10, 100, 100), "Bring"))
            {
                Decrypt();
            }

            if (GUI.Button(new Rect(210, 10, 100, 100), "Text"))
            {
                using (AesString aes = new AesString())
                {
                    Debug.Log($"plainText: {plainText}");
                    aes.SetKey(caKey).SetIv(caIv);
                    string cipherText = aes.Encrypt(plainText);
                    Debug.Log($"cipherText: {cipherText}");
                    string decryptedText = aes.Decrypt(cipherText);
                    Debug.Log($"decryptedText: {decryptedText}");
                }
            }

            if (GUI.Button(new Rect(310, 10, 100, 100), "SHA-256"))
            {
                SHA256Encryptor sha256 = new SHA256Encryptor();
                string hash = string.IsNullOrEmpty(salt) ? sha256.Encrypt(plainText) : sha256.Encrypt(plainText, salt, iteration);
                Debug.Log($"hash: {hash}");
            }
        }

        /// <summary>
        /// byte array를 받아 Texture2D로 변환하는 함수
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <returns>Texture2D</returns>
        private Texture2D ByteArrayToTexture2D(byte[] bytes)
        {
            // 바이트가 없는 경우 null반환
            if (bytes == null)
                return null;
            // 바이트가 있는 경우
            else
            {
                // 텍스쳐2D 생성 및 속성 설정
                Texture2D tex2D = new Texture2D(0, 0);
                tex2D.wrapMode = TextureWrapMode.Clamp;
                tex2D.filterMode = FilterMode.Bilinear;

                // byte 이미지 변환
                tex2D.LoadImage(bytes);

                // 이미지 반환
                return tex2D;
            }
        }
    }
}