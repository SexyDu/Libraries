#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class TouchTarget : MonoBehaviour, ITouchTarget
    {
        public abstract void AddTouch(int fingerId);

        public abstract void ClearTouch();

        protected TouchConfig Config => TouchCenter.Config;

        /// <summary>
        /// Unity Position Per One Pixel
        /// TouchCenter가 송출하는 화면의 1 픽셀 당 유니티 위치(크기) 값
        /// </summary>
        protected float UPPOP => Config.UPPOP;

        protected Vector2 GetTouchPosition(int fingerId)
        {
            return Config.GetTouchPosition(fingerId);
        }
    }
}