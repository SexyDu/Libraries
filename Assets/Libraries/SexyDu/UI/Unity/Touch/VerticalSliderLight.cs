using System.Collections;
using UnityEngine;

namespace SexyDu.UI.Unity
{
    public sealed class VerticalSliderLight : TouchTargetBasic
    {
        public override void AddTouch(int fingerId)
        {
            StartSlide(fingerId);
        }

        [SerializeField] private RectTransform target;

        #region Slide
        [SerializeField] private float min;
        [SerializeField] private float max;

        private void StartSlide(int fingerId)
        {
            EndSlide();

            IeSlide = CoSlide(fingerId);
            StartCoroutine(IeSlide);
        }

        private void EndSlide()
        {
            if (IeSlide != null)
            {
                StopCoroutine(IeSlide);
                IeSlide = null;
            }
        }

        private IEnumerator IeSlide = null;
        private IEnumerator CoSlide(int fingerId)
        {
            Vector2 prev = GetTouchPosition(fingerId);

            if (prev.Equals(Vector2.zero))
                yield break;

            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                Vector2 current = GetTouchPosition(fingerId);

                if (current.Equals(Vector2.zero))
                {
                    break;
                }
                else
                {
                    float deltaPosition = current.y - prev.y;
                    anchoredPosition.y += deltaPosition;

                    target.anchoredPosition = anchoredPosition;

                    prev = current;
                }

            } while (true);
        }
        #endregion
    }
}