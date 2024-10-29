using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using SexyDu.Touch;

namespace SexyDu.UI
{
    public abstract class ButtonHandler : TouchTarget
    {
        #region Touch
        protected int fingerId = int.MinValue;

        protected bool Touched => !fingerId.Equals(int.MinValue);

        /// <summary>
        /// 터치 입력 함수
        /// : ITouchTarget
        /// </summary>
        public override void AddTouch(int fingerId)
        {
            if (Touched)
                this.fingerId = fingerId;
        }
        
        /// <summary>
        /// 터치 클리어
        /// : ITouchTarget
        /// </summary>
        public override void ClearTouch()
        {
            ClearFingerID();

            InteractUp();
        }

        protected virtual void ClearFingerID()
        {
            fingerId = int.MinValue;
        }
        #endregion

        #region Interactable
        [SerializeField] protected bool interactable = true;
        public void SetInteracableStatus(bool interactable)
        {
            this.interactable = interactable;
        }
        #endregion

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        protected Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
        public Button.ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        #region Interaction
        [SerializeField] private ButtonInteract[] interacts;

        protected void InteractPress()
        {
            for (int i = 0; i < interacts.Length; i++)
            {
                interacts[i].OnButtonPress();
            }
        }
        protected void InteractUp()
        {
            for (int i = 0; i < interacts.Length; i++)
            {
                interacts[i].OnButtonUp();
            }
        }

#if UNITY_EDITOR
        public void SetInteracts()
        {
            ButtonInteract[] interacts = GetComponents<ButtonInteract>();
            this.interacts = interacts;
        }
#endif

        #endregion
    }
}