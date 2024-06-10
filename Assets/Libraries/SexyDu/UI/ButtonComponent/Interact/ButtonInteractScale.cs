using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractScale : ButtonInteract
    {
        public override void OnButtonPress()
        {
            SetObjectScale(scale_press);
        }

        public override void OnButtonUp()
        {
            SetObjectScale(scale_normal);
        }

        [SerializeField] private Transform target;
        [SerializeField] private Vector3 scale_normal;
        [SerializeField] private Vector3 scale_press;

        private void SetObjectScale(Vector3 scale)
        {
            target.localScale = scale;
        }
    }
}