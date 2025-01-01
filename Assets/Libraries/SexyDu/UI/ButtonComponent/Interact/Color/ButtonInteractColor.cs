using UnityEngine;

namespace SexyDu.UI
{
    public abstract class ButtonInteractColor : ButtonInteract
    {
        [SerializeField] protected Color[] cols_normal;
        [SerializeField] protected Color[] cols_press;
        
        public override void OnButtonPress()
        {
            SetRendersColor(cols_press);
        }

        public override void OnButtonUp()
        {
            SetRendersColor(cols_normal);
        }

        protected abstract void SetRendersColor(Color[] cols);
    }
}