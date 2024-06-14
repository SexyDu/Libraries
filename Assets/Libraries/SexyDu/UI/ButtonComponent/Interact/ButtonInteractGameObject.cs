using UnityEngine;

namespace SexyDu.UI
{
    public class ButtonInteractGameObject : ButtonInteract
    {
        public override void OnButtonPress()
        {
            SetObjectsActive(true);
        }

        public override void OnButtonUp()
        {
            SetObjectsActive(false);
        }

        [SerializeField] private GameObject[] objs;

        private void SetObjectsActive(bool active)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(active);
            }
        }
    }
}