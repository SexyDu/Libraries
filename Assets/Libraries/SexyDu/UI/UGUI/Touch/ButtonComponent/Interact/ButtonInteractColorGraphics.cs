using UnityEngine;
using UnityEngine.UI;

namespace SexyDu.UI.UGUI
{
    public class ButtonInteractColorGraphics : ButtonInteractColor
    {
        [SerializeField] private Graphic[] graphics;

        protected override void SetRendersColor(Color[] cols)
        {
            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].color = cols[i];
            }
        }

#if UNITY_EDITOR
        public override void SetDefaultColors()
        {
            if (graphics == null || graphics.Length.Equals(0))
                throw new System.Exception("해당 스크립트에 연결된 images가 없습니다.");

            cols_normal = new Color[graphics.Length];
            cols_press = new Color[graphics.Length];

            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i] == null)
                {
                    cols_normal = null;
                    cols_press = null;

                    throw new System.NullReferenceException("images 배열에 null이 있습니다.");
                }
                else
                    cols_normal[i] = cols_press[i] = graphics[i].color;
            }
        }
#endif
    }
}