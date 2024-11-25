using System;
using System.Text;
using UnityEngine;

namespace SexyDu.Crypto
{
    public class Sample : MonoBehaviour
    {
        private AesEncryptor aes = new AesEncryptor();

        [SerializeField] private string inputFile;
        [SerializeField] private string outputFile;
        [SerializeField] private Texture2D texture2D;
// .// GPT 답변 보고 수정하자
        private const string KeyStr = "generalsexy.secretkey18@song.com";
        private const string IvStr = "sexy.iv@song.com";
        
        // 2PqHQ/DxV5QxMM9RH6fh7Z7pSSNbqsauwQVG25d4IyU=
        private readonly char[] caKey = new char[44] { '2', 'P', 'q', 'H', 'Q', '/', 'D', 'x', 'V', '5', 'Q', 'x', 'M', 'M', '9', 'R', 'H', '6', 'f', 'h', '7', 'Z', '7', 'p', 'S', 'S', 'N', 'b', 'q', 's', 'a', 'u', 'w', 'Q', 'V', 'G', '2', '5', 'd', '4', 'I', 'y', 'U', '=' };
        // lR3ynui08Gqq4gmkA3osNA==
        private readonly char[] caIv = new char[24] { 'l', 'R', '3', 'y', 'n', 'u', 'i', '0', '8', 'G', 'q', 'q', '4', 'g', 'm', 'k', 'A', '3', 'o', 's', 'N', 'A', '=', '=' };

        private void Encrypt()
        {
            byte[] key = Convert.FromBase64CharArray(caKey, 0, caKey.Length);
            byte[] iv = Convert.FromBase64CharArray(caIv, 0, caIv.Length);
            aes.Encrypt(inputFile, outputFile, key, iv);
            Debug.LogFormat("key : {0} bytes\niv : {1} bytes", key.Length, iv.Length);
        }

        private void Decrypt()
        {
            byte[] key = Convert.FromBase64CharArray(caKey, 0, caKey.Length);
            byte[] iv = Convert.FromBase64CharArray(caIv, 0, caIv.Length);
            byte[] data = aes.Decrypt(outputFile, key, iv);
            texture2D = ByteArrayToTexture2D(data);
        }

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