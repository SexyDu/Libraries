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

        protected virtual Vector2 GetTouchPosition(int fingerId)
        {

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId.Equals(fingerId))
                    return Input.touches[i].position;
            }

#if CONSIDER_MOUSE
            switch (fingerId)
            {
                case TouchCenter.MouseIdLeft:
                    if (Input.GetMouseButton(0))
                        return Input.mousePosition;
                    else
                        break;
                case TouchCenter.MouseIdRight:
                    if (Input.GetMouseButton(1))
                        return Input.mousePosition;
                    else
                        break;
            }
#endif

            return Vector2.zero;
        }
    }
}