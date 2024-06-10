using System.Collections;
using UnityEngine;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    public sealed class VerticalSliderLight : TouchTarget
    {
        #region OnAwakeInit
        [SerializeField] private bool onAwakeInit;

        private void Awake()
        {
            if (onAwakeInit)
                Initialize();
        }

        private VerticalSliderLight Initialize()
        {
            // 상위 Transform으로부터 Canvas를 찾아서 초기설정 하는 루틴
            Transform canvasTarget = this.transform.parent;
            while (canvasTarget != null)
            {
                Canvas canvas = canvasTarget.GetComponent<Canvas>();
                if (canvas != null)
                    return Initialize(canvas);
                else
                    canvasTarget = canvasTarget.parent;
            }

            Debug.LogErrorFormat("'{0}'(VerticalSliderLight) 상위에 Canvas를 찾지 못하여 정상적으로 동잘하기 않을 수 있습니다.", name);
            return Initialize(1);
        }
        #endregion

        public VerticalSliderLight Initialize(Canvas canvas)
        {
            return Initialize(canvas.scaleFactor);
        }

        public VerticalSliderLight Initialize(float scaleFactor)
        {
            scaleFactorForMult = 1f / scaleFactor;

            return this;
        }

        public override void AddTouch(int fingerId)
        {
            StartSlide(fingerId);
        }

        public override void ClearTouch()
        {
            EndSlide();
        }

        [SerializeField] private RectTransform target;
        private float scaleFactorForMult = 1f;

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

            if (UseInertia)
            {
                if (inertiaQueue == null)
                    inertiaQueue = new InertiaQueue(inertiaFrameCount);
                else
                    inertiaQueue.ClearQueue();
            }

            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                Vector2 current = GetTouchPosition(fingerId);

                if (current.Equals(Vector2.zero))
                {
                    StartInertia(inertiaQueue.PositionPerOneSecond());
                    inertiaQueue.ClearQueue();
                }
                else
                {
                    float deltaPosition = (current.y - prev.y) * scaleFactorForMult;
                    anchoredPosition.y += deltaPosition;

                    target.anchoredPosition = anchoredPosition;

                    inertiaQueue?.Enqueue(deltaPosition, Time.deltaTime);

                    prev = current;
                }

            } while (true);
        }
        #endregion

        #region Inertia
        [Header("Inertia")]
        [SerializeField] private int inertiaFrameCount; // 관성 계산에 사용될 queue의 수
        [SerializeField] private float decelerationRate; // 감속 수치
        [SerializeField] private float limitInertia; // 최대 광성 수치
        [SerializeField] private float inertiaBreak; // 최소 감속 수치
        private bool UseInertia => inertiaFrameCount > 0; // 관성 사용 여부
        private bool HasLimitInertia => limitInertia > 0; // 관성 제한값 설정 여부

        // 관성 관리 queue
        private InertiaQueue inertiaQueue = null;

        private void StartInertia(float inertiaPerOneSec)
        {
            Debug.LogFormat("StartInertia({0})", inertiaPerOneSec);
            EndSlide();

            if (inertiaPerOneSec > inertiaBreak)
            {
                if (HasLimitInertia && inertiaPerOneSec > limitInertia)
                    inertiaPerOneSec = limitInertia;

                IeSlide = CoInertiaIncrease(inertiaPerOneSec);
                StartCoroutine(IeSlide);
            }
            else if (inertiaPerOneSec < -inertiaBreak)
            {
                if (HasLimitInertia && inertiaPerOneSec < -limitInertia)
                    inertiaPerOneSec = -limitInertia;

                IeSlide = CoInertiaDecrease(inertiaPerOneSec);
                StartCoroutine(IeSlide);
            }
        }

        private IEnumerator CoInertiaDecrease(float inertiaPerOneSec)
        {
            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                float deltaTime = Time.deltaTime;
                anchoredPosition.y += inertiaPerOneSec * deltaTime;
                target.anchoredPosition = anchoredPosition;

                inertiaPerOneSec += decelerationRate / deltaTime;
                Debug.LogFormat("inertiaPerOneSec : {0}", inertiaPerOneSec);
                
            } while (inertiaPerOneSec < -inertiaBreak);
        }

        private IEnumerator CoInertiaIncrease(float inertiaPerOneSec)
        {
            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                float deltaTime = Time.deltaTime;
                anchoredPosition.y += inertiaPerOneSec * deltaTime;
                target.anchoredPosition = anchoredPosition;

                inertiaPerOneSec -= decelerationRate / deltaTime;
                Debug.LogFormat("inertiaPerOneSec : {0}", inertiaPerOneSec);

            } while (inertiaPerOneSec > inertiaBreak);
        }
        #endregion
    }
}