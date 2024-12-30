using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractAnchoredPosition : ButtonInteract
    {
        public override void OnButtonPress()
        {
            SetObjectPosition(pos_press);
        }

        public override void OnButtonUp()
        {
            SetObjectPosition(pos_normal);
        }

        [SerializeField] private RectTransform target;
        [SerializeField] private Vector2 pos_normal;
        [SerializeField] private Vector2 pos_press;

        private void SetObjectPosition(Vector2 pos)
        {
            target.anchoredPosition = pos;
        }
    }
}