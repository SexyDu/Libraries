using System.Collections;
using UnityEngine;
using SexyDu.Touch;

namespace SexyDu.UI.UGUI
{
    /// <summary>
    /// 가벼운 수직 스크롤
    /// </summary>
    public sealed class VerticalSliderLight : TouchTarget
    {
        #region OnAwakeInit
        [SerializeField] private bool onAwakeInit;

        private void Awake()
        {
            if (onAwakeInit)
                Initialize();
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
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

        /// <summary>
        /// 초기 설정
        /// </summary>
        public VerticalSliderLight Initialize(Canvas canvas)
        {
            return Initialize(canvas.scaleFactor);
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
        public VerticalSliderLight Initialize(float scaleFactor)
        {
            scaleFactorForMult = 1f / scaleFactor;

            return this;
        }

        /// <summary>
        /// 터치 추가
        /// : TouchTarget : ITouchTarget
        /// </summary>
        public override void AddTouch(int fingerId)
        {
            StartSlide(fingerId);
        }

        /// <summary>
        /// 터치 클리어
        /// : TouchTarget : ITouchTarget
        /// </summary>
        public override void ClearTouch()
        {
            EndSlide();
        }

        /// <summary>
        /// 하위 Sender의 Receiver에 해당 슬라이더 연결
        /// </summary>
        public void ConnectSendersInChildren()
        {
            ITouchTargetSender[] senders = GetComponentsInChildren<ITouchTargetSender>();
            if (senders != null)
            {
                for (int i = 0; i < senders.Length; i++)
                {
                    senders[i].SetTouchReceiver(this);
                }
            }
        }

        // 슬라이드 대상
        [SerializeField] private RectTransform target;
        // 터치 보정용 Canvas scale factor
        private float scaleFactorForMult = 1f;

        #region Slide
        [SerializeField] private float min; // slide 최솟값
        [SerializeField] private float max; // slide 최댓값

        /// <summary>
        /// slide 최솟값 설정
        /// </summary>
        public void SetMinimum(float min)
        {
            this.min = min;
        }
        /// <summary>
        /// slide 최댓값 설정
        /// </summary>
        public void SetMaximum(float max)
        {
            this.max = max;
        }

        /// <summary>
        /// 슬라이더 실행
        /// </summary>
        private void StartSlide(int fingerId)
        {
            EndSlide();

            IeSlide = CoSlide(fingerId);
            StartCoroutine(IeSlide);
        }

        /// <summary>
        /// 슬라이더 종료
        /// </summary>
        private void EndSlide()
        {
            if (IeSlide != null)
            {
                StopCoroutine(IeSlide);
                IeSlide = null;
            }
        }

        /// <summary>
        /// 슬라이더 코루틴
        /// </summary>
        private IEnumerator IeSlide = null;
        private IEnumerator CoSlide(int fingerId)
        {
            Vector2 prev = GetTouchPosition(fingerId);

            // 터치 포지션을 찾지 못한 경우 종료
            if (prev.Equals(Vector2.zero))
                yield break;

            // 관성 사용인 경우 초기화
            if (UseInertia)
            {
                if (inertiaQueue == null)
                    inertiaQueue = new InertiaQueue(inertiaFrameCount);
                else
                    inertiaQueue.ClearQueue();
            }

            // 초기 타겟 위치
            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                Vector2 current = GetTouchPosition(fingerId);

                // 현재 위치값을 찾지 못한 경우(터치 종료)
                if (current.Equals(Vector2.zero))
                {
                    // 관성 코루틴 실행
                    StartInertia(inertiaQueue.PositionPerOneSecond());
                    // 관성 데이터 클리어
                    inertiaQueue.ClearQueue();
                }
                else
                {
                    // deltaPosition 계산 및 적용
                    float deltaPosition = (current.y - prev.y) * scaleFactorForMult;
                    anchoredPosition.y += deltaPosition;

                    target.anchoredPosition = anchoredPosition;

                    // 관성 queue에 현재 delta 데이터 입력
                    inertiaQueue?.Enqueue(deltaPosition, Time.deltaTime);

                    // 이전값을 현재값으로 변경
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

        /// <summary>
        /// 관성 코루틴 실행
        /// </summary>
        private void StartInertia(float inertiaPerOneSec)
        {
            // 기존 코루틴 종료
            EndSlide();

            // 증가상태인 경우
            if (inertiaPerOneSec > inertiaBreak)
            {
                // 관성 제한값이 있는 경우 보정
                if (HasLimitInertia && inertiaPerOneSec > limitInertia)
                    inertiaPerOneSec = limitInertia;
                // 코루틴 실행
                IeSlide = CoInertiaIncrease(inertiaPerOneSec);
                StartCoroutine(IeSlide);
            }
            // 감소상태인 경우
            else if (inertiaPerOneSec < -inertiaBreak)
            {
                // 관성 제한값이 있는 경우 보정
                if (HasLimitInertia && inertiaPerOneSec < -limitInertia)
                    inertiaPerOneSec = -limitInertia;
                // 코루틴 실행
                IeSlide = CoInertiaDecrease(inertiaPerOneSec);
                StartCoroutine(IeSlide);
            }
        }

        /// <summary>
        /// 증가 관성 코루틴
        /// </summary>
        private IEnumerator CoInertiaDecrease(float inertiaPerOneSec)
        {
            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                // 관성 수치 적용
                float deltaTime = Time.deltaTime;
                anchoredPosition.y += inertiaPerOneSec * deltaTime;
                target.anchoredPosition = anchoredPosition;

                // 감속처리
                inertiaPerOneSec += decelerationRate / deltaTime;
                
            } while (inertiaPerOneSec < -inertiaBreak);
        }
        /// <summary>
        /// 감소 관성 코루틴
        /// </summary>
        private IEnumerator CoInertiaIncrease(float inertiaPerOneSec)
        {
            Vector2 anchoredPosition = target.anchoredPosition;

            do
            {
                yield return null;

                // 관성 수치 적용
                float deltaTime = Time.deltaTime;
                anchoredPosition.y += inertiaPerOneSec * deltaTime;
                target.anchoredPosition = anchoredPosition;

                // 감속처리
                inertiaPerOneSec -= decelerationRate / deltaTime;

            } while (inertiaPerOneSec > inertiaBreak);
        }
        #endregion
    }
}