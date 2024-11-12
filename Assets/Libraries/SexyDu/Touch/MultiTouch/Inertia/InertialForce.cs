using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 관성력 데이터
    /// </summary>
    public struct InertialForce : IClearable
    {
        // 이동 거리
        public Vector2 deltaPosition
        {
            get;
            private set;
        }
        // 이동 시간
        public float deltaTime
        {
            get;
            private set;
        }
        /// <summary>
        /// 유효성
        /// </summary>
        public bool Available
        {
            get
            {
                return deltaPosition != Vector2.zero && deltaTime > 0f;
            }
        }
        /// <summary>
        /// 데이터간 크기 비교를 위한 절댓값
        /// </summary>
        public float Absolute => Mathf.Abs(deltaPosition.x) + Mathf.Abs(deltaPosition.y);
        /// <summary>
        /// 관성력 정보 설정
        /// </summary>
        public void Set(Vector2 deltaPosition, float deltaTime)
        {
            this.deltaPosition = deltaPosition;
            this.deltaTime = deltaTime;
        }
        /// <summary>
        /// 관성력 정보 클리어
        /// </summary>
        public void Clear()
        {
            Set(Vector2.zero, 0f);
        }
    }
}
