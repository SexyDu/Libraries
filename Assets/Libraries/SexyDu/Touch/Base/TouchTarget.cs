#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치 대상 기본 클래스
    /// </summary>
    public abstract class TouchTarget : MonoBehaviour, ITouchTarget
    {
        /// <summary>
        /// 터치된 fingerId 수신
        /// </summary>
        /// <param name="fingerId"></param>
        public abstract void ReceiveTouch(int fingerId);

        /// <summary>
        /// 터치 클리어
        /// </summary>
        public abstract void ClearTouch();

        protected TouchConfig Config => TouchCenter.Config;

        /// <summary>
        /// Unity Position Per One Pixel
        /// TouchCenter가 송출하는 화면의 1 픽셀 당 유니티 위치(크기) 값
        /// </summary>
        protected float UPPOP => Config.UPPOP;

        /// <summary>
        /// fingerId에 해당하는 터치의 위치값
        /// </summary>
        protected Vector2 GetTouchPosition(int fingerId)
        {
            return Config.GetTouchPosition(fingerId);
        }
    }
}