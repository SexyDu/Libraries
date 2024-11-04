using UnityEngine;
using UnityEngine.Events;
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
        // 버튼 동작 수행 여부
        [SerializeField] protected bool interactable = true;

        /// <summary>
        /// 버튼 동작 수행 여부 설정
        /// </summary>
        public void SetInteracableStatus(bool interactable)
        {
            this.interactable = interactable;
        }
        #endregion

        #region Event
        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        protected UnityEvent m_OnClick = new UnityEvent();
        public UnityEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        protected virtual void OnClick()
        {
            m_OnClick?.Invoke();
        }
        #endregion

        #region Interaction
        // 버튼 터치 인터렉션
        [SerializeField] private ButtonInteract[] interacts;

        /// <summary>
        /// 버튼 눌림 인터렉션
        /// </summary>
        protected void InteractPress()
        {
            for (int i = 0; i < interacts.Length; i++)
            {
                interacts[i].OnButtonPress();
            }
        }

        /// <summary>
        /// 버튼 눌림 해제 인터렉션
        /// </summary>
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