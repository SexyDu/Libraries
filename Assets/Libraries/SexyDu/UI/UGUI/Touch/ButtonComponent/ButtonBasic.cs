using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    /// <summary>
    /// 기본 버튼
    /// </summary>
    public class ButtonBasic : TouchTarget, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                ReceiveTouch(eventData.pointerId);
                InteractPress();
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            InteractUp();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
            {
                if (FingerId.Equals(eventData.pointerId))
                {
                    m_OnClick.Invoke();
                    ClearFingerID();
                }
            }
        }

        protected virtual void OnDisable()
        {
            ClearFingerID();

            InteractUp();
        }

        #region Touch
        private int fingerId = int.MinValue;
        protected int FingerId => fingerId;

        public override void ReceiveTouch(int fingerId)
        {
            this.fingerId = fingerId;
        }

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
        private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
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