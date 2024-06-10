using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SexyDu.UI.Unity
{
    public class ButtonTouchTargetSender : ButtonBasic
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
                    ReleaseTouch();
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopTouch();
        }

        [Header("Sender")]
        [SerializeField] private TouchTargetBasic transferTarget;

        private void SendTouch()
        {
            if (transferTarget != null)
            {
                transferTarget.AddTouch(FingerId);
                ReleaseTouch();
            }
        }

        private void ReleaseTouch()
        {
            InteractUp();
            ClearFingerID();
            StopTouch();
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
            float compareDistance = Screen.height * 0.01f;

            if (initial.Equals(Vector2.zero))
                ReleaseTouch();

            do
            {
                yield return null;

                Vector2 current = GetTouchPosition(FingerId);

                if (current.Equals(Vector2.zero))
                {
                    ReleaseTouch();
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