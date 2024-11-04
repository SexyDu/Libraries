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

        protected Vector2 GetTouchPosition(int fingerId)
        {
            return Config.GetTouchPosition(fingerId);
        }
    }
}