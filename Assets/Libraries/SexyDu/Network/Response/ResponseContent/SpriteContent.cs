using UnityEngine;

namespace SexyDu.Network
{
    /// <summary>
    /// Sprite 수신 컨텐츠
    ///  * Sprite는 파괴해도 안에 남아있는 texture는 살아있기 때문에 함께 파괴해줘야 한다.
    /// </summary>
    public class SpriteContent : IReleasableResponseContent
    {
        public readonly Sprite _sprite = null;
        public Sprite sprite => _sprite;

        public SpriteContent(Sprite sprite)
        {
            _sprite = sprite;
        }

        public SpriteContent(Texture2D tex)
        {
            _sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public void Release()
        {
            if (sprite != null)
            {
                if (sprite.texture != null)
                    Object.Destroy(sprite.texture);
                Object.Destroy(sprite);
            }
        }
    }
}