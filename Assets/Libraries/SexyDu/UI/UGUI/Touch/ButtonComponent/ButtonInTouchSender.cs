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
                // onDownClearReceiver 활성화의 경우 down시 receiver의 터치를 멈추도록 설정
                if (onDownClearReceiver)
                    touchReceiver?.ClearTouch();
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

        #region Sender
        [Header("Sender")]
        [SerializeField] private TouchTarget touchReceiver; // 터치 리시버
        [SerializeField] private bool onDownClearReceiver; // 버튼 입력 시 리시버의 터치를 클리어시킬 지 여부

        /// <summary>
        /// 터치 리시버 설정
        /// : ITouchTargetSender
        /// </summary>
        public void SetTouchReceiver(ITouchTarget receiver)
        {
            if (receiver is TouchTarget)
                touchReceiver = receiver as TouchTarget;
            else
                Debug.LogError("TouchTargetSender에서는 TouchTarget만을 Receiver로 받을 수 있습니다.");
        }

        /// <summary>
        /// 리시버에 터치 전달
        /// </summary>
        private void SendTouch()
        {
            if (touchReceiver != null)
            {
                touchReceiver.AddTouch(FingerId);
                ClearTouch();
            }
        }
        #endregion

        /// <summary>
        /// 터치 확인 루틴 실행
        /// </summary>
        private void StartTouch()
        {
            StopTouch();

            IeTouch = CoTouch();

            StartCoroutine(IeTouch);
        }

        /// <summary>
        /// 터치 확인 루틴 종료
        /// </summary>
        private void StopTouch()
        {
            if (IeTouch != null)
            {
                StopCoroutine(IeTouch);
                IeTouch = null;
            }
        }

        /// <summary>
        /// 터치 확인 루틴
        /// </summary>
        private IEnumerator IeTouch = null;
        private IEnumerator CoTouch()
        {
            Vector2 initial = GetTouchPosition(FingerId);

            if (initial.Equals(Vector2.zero))
                ClearTouch();

            // 터치 이동 비교 대상
            /// 일반적으로 Screen.height의 1%가 적당
            /// TODO: 아래 코드 범용적으로 변경
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

        /// <summary>
        /// 비교 대상을 넘어선 수치가 있는지 여부 반환
        /// </summary>
        private bool IsLeave(float compare, Vector2 distance)
        {
            distance = Abs(distance);
            return distance.x > compare || distance.y > compare;
        }

        /// <summary>
        /// Vector2 절대값
        /// </summary>
        private Vector2 Abs(Vector2 distance)
        {
            if (distance.x < 0f)
                distance.x *= -1f;
            if (distance.y < 0f)
                distance.y *= -1f;

            return distance;
        }
    }
}