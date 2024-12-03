using UnityEngine;

namespace SexyDu.Tool
{
    public static class ConvertFromBytes
    {
        /// <summary>
        /// byte array를 받아 Texture2D로 변환하는 함수
        /// </summary>
        public static Texture2D ToTexture2D(byte[] bytes)
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

        /// <summary>
        /// byte array를 받아 Sprite로 변환하는 함수
        /// </summary>
        public static Sprite ToSprite(byte[] bytes)
        {
            Texture2D tex2D = ToTexture2D(bytes);
            return Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));
        }
    }
}