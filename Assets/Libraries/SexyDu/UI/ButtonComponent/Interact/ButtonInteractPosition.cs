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
    }
}