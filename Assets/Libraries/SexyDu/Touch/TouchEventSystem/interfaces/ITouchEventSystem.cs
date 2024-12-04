using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치 이벤트 시스템
    /// </summary>
    public interface ITouchEventSystem
    {
        /// <summary>
        /// 터치 수신자 등록
        /// </summary>
        public void Subscribe(ITouchEventReceiver receiver);
        /// <summary>
        /// 터치 수신자 제거
        /// </summary>
        public void Unsubscribe(ITouchEventReceiver receiver);
        /// <summary>
        /// 터치 수신자 클리어
        /// </summary>
        public void ClearSubscription();
    }
}