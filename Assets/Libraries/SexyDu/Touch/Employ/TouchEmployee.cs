#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_DESKTOP
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치의 추가 기능(더블클릭, 지속터치액션 등) 구현 시 사용될 클래스
    /// </summary>
    public abstract class TouchEmployee : MonoBehaviour, ITouchEmployee
    {
        protected ITouchEmployer employer = null;

        /// <summary>
        /// 고용인 등록
        /// </summary>
        public virtual void SetEmpoloyer(ITouchEmployer employer)
        {
            this.employer = employer;
        }

        /// <summary>
        /// 터치 입력 감지
        /// </summary>
        public virtual void Detect(int fingerId)
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId.Equals(fingerId))
                {
                    Detect(fingerId, Input.touches[i].position);
                    return;
                }
            }

#if CONSIDER_DESKTOP
            if (ITouchCenter.Config.IsMouse(fingerId))
                Detect(fingerId, Input.mousePosition);
#endif
        }

        /// <summary>
        /// 터치 입력 감지
        /// </summary>
        public virtual void Detect(int fingerId, Vector2 pos)
        {
            Detect(fingerId, pos, Time.time);
        }

        /// <summary>
        /// 터치 입력 감지
        /// </summary>
        public abstract void Detect(int fingerId, Vector2 pos, float time);

        /// <summary>
        /// 터치 입력 해제
        /// </summary>
        public abstract void Disappear();

        /// <summary>
        /// 터치 입력 취소
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// 동작 수행 보고
        /// </summary>
        protected void Report()
        {
            employer?.ReceiveReport();
        }

        #region Event
        // Event delegates triggered on click.
        [FormerlySerializedAs("onEvent")]
        [SerializeField]
        protected UnityEvent m_OnEvent = new UnityEvent();
        public UnityEvent onEvent
        {
            get { return m_OnEvent; }
            set { m_OnEvent = value; }
        }

        protected virtual void OnEvent()
        {
            m_OnEvent?.Invoke();

            Report();

            Cancel();
        }
        #endregion
    }
}