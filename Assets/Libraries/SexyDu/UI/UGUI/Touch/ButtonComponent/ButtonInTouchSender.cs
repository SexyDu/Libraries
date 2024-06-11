using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    /// <summary>
    /// 버튼
    /// * 버튼 터치에 이동 감지 시 지정된 sender로 이관
    /// </summary>
    public class ButtonInTouchSender : ButtonBasic, ITouchTargetSender
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                AddTouch(eventData.pointerId);
                InteractPress();

                StartTouch();
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            // NotUsed
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
            {
                if (FingerId.Equals(eventData.pointerId))
                {
                    onClick.Invoke();
                    ClearTouch();
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopTouch();
        }

        public override void ClearTouch()
        {
            base.ClearTouch();

            StopTouch();
        }

        [Header("Sender")]
        [SerializeField] private TouchTarget touchReceiver;

        public void SetTouchReceiver(ITouchTarget receiver)
        {
            if (receiver is TouchTarget)
                touchReceiver = receiver as TouchTarget;
            else
                Debug.LogError("TouchTargetSender에서는 TouchTarget만을 Receiver로 받을 수 있습니다.");
        }

        private void SendTouch()
        {
            if (touchReceiver != null)
            {
                touchReceiver.AddTouch(FingerId);
                ClearTouch();
            }
        }

        private void StartTouch()
        {
            StopTouch();

            IeTouch = CoTouch();

            StartCoroutine(IeTouch);
        }

        private void StopTouch()
        {
            if (IeTouch != null)
            {
                StopCoroutine(IeTouch);
                IeTouch = null;
            }
        }

        private IEnumerator IeTouch = null;
        private IEnumerator CoTouch()
        {
            Vector2 initial = GetTouchPosition(FingerId);
            
            if (initial.Equals(Vector2.zero))
                ClearTouch();

            float compareDistance = Screen.height * 0.01f;

            do
            {
                yield return null;

                Vector2 current = GetTouchPosition(FingerId);

                if (current.Equals(Vector2.zero))
                {
                    ClearTouch();
                }
                else
                {
                    Vector2 distance = current - initial;
                    if (IsLeave(compareDistance, distance))
                        SendTouch();
                }
            } while (true);
        }

        private bool IsLeave(float compare, Vector2 distance)
        {
            return compare < distance.x || -compare > distance.x
                || compare < distance.y || -compare > distance.y;
        }
    }
}