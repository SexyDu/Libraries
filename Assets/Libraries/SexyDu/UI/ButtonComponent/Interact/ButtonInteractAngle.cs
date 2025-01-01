using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractAngle : ButtonInteract
    {
        public override void OnButtonPress()
        {
            SetObjectScale(press);
        }

        public override void OnButtonUp()
        {
            SetObjectScale(normal);
        }

        [SerializeField] private Transform target;
        [SerializeField] private Vector3 normal;
        [SerializeField] private Vector3 press;

        private void SetObjectScale(Vector3 scale)
        {
            target.localEulerAngles = scale;
        }

#if UNITY_EDITOR
        public override void ConstructDefaultSetting()
        {
            if (target == null)
            {
                throw new System.NullReferenceException("target이 존재하지 않습니다.");
            }

            normal = press = target.localEulerAngles;
        }
#endif
    }
}