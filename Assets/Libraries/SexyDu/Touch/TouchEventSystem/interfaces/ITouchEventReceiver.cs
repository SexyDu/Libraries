using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치 이벤트 수신자
    /// </summary>
    public interface ITouchEventReceiver
    {
        /// <summary>
        /// 터치 시작 이벤트
        /// </summary>
        void OnTouchBegin(UnityEngine.Touch touch);
        /// <summary>
        /// 마우스 시작 이벤트
        /// </summary>
        void OnMouseBegin(int mouseId, Vector2 position);
    }
}