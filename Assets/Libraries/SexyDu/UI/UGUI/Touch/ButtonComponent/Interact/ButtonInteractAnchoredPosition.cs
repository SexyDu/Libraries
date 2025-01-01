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

#if UNITY_EDITOR
        public override void ConstructDefaultSetting()
        {
            if (target == null)
            {
                throw new System.NullReferenceException("target이 존재하지 않습니다.");
            }

            pos_normal = pos_press = target.anchoredPosition;
        }
#endif
    }
}