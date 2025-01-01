using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractPosition : ButtonInteract
    {
        public override void OnButtonPress()
        {
            SetObjectPosition(pos_press);
        }

        public override void OnButtonUp()
        {
            SetObjectPosition(pos_normal);
        }

        [SerializeField] private Transform target;
        [SerializeField] private Vector3 pos_normal;
        [SerializeField] private Vector3 pos_press;

        private void SetObjectPosition(Vector3 pos)
        {
            target.localPosition = pos;
        }

#if UNITY_EDITOR
        public override void ConstructDefaultSetting()
        {
            if (target == null)
            {
                throw new System.NullReferenceException("target이 존재하지 않습니다.");
            }

            pos_normal = pos_press = target.localPosition;
        }
#endif
    }
}