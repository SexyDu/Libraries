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
    public class ButtonForTouchSender : ButtonBasic
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
            //InteractUp();
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
        [SerializeField] private TouchTarget transferTarget;

        private void SendTouch()
        {
            if (transferTarget != null)
            {
                transferTarget.AddTouch(FingerId);
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