using UnityEngine;

namespace SexyDu.UI
{
    /// <summary>
    /// 버튼 상호작용
    /// </summary>
    public abstract class ButtonInteract : MonoBehaviour, IButtonInteract
    {
        public abstract void OnButtonPress();

        public abstract void OnButtonUp();
    }
}