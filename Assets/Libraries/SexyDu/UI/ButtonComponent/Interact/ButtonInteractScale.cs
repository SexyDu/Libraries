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

#if UNITY_EDITOR
        public override void ConstructDefaultSetting()
        {
            if (target == null)
            {
                throw new System.NullReferenceException("target이 존재하지 않습니다.");
            }

            scale_normal = scale_press = target.localScale;
        }
#endif
    }
}