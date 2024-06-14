using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractColorSprites : ButtonInteractColor
    {
        [SerializeField] private SpriteRenderer[] renders;

        protected override void SetRendersColor(Color[] cols)
        {
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].color = cols[i];
            }
        }

#if UNITY_EDITOR
        public override void SetDefaultColors()
        {
            if (renders == null || renders.Length.Equals(0))
                throw new System.Exception("해당 스크립트에 연결된 renders가 없습니다.");

            cols_normal = new Color[renders.Length];
            cols_press = new Color[renders.Length];

            for (int i = 0; i < renders.Length; i++)
            {
                if (renders[i] == null)
                {
                    cols_normal = null;
                    cols_press = null;

                    throw new System.NullReferenceException("renders 배열에 null이 있습니다.");
                }
                else
                    cols_normal[i] = cols_press[i] = renders[i].color;
            }
        }
#endif
    }
}