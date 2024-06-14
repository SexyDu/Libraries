using System;
using UnityEngine;
using UnityEngine.UI;

namespace SexyDu.UI.UGUI
{
    [Serializable]
    public struct NullableImage
    {
        public NullableImage(Image image)
        {
            this.image = image;
        }

        [SerializeField] private Image image;
        public Image Image { get => image; }

        public Sprite sprite
        {
            set
            {
                image.sprite = value;

                SetColor();
            }
        }

        public void SetSprite(Sprite sprite)
        {
            this.sprite = sprite;
        }

        private void SetColor()
        {
            image.color = image.sprite == null ? Color.clear : Color.white;
        }
    }
}