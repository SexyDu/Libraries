using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class TouchTarget : MonoBehaviour, ITouchTarget
    {
        public abstract void AddTouch(int fingerId);

        public abstract void ClearTouch();

        protected Vector2 GetTouchPosition(int fingerId)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId.Equals(fingerId))
                    return Input.touches[i].position;
            }

            return Vector2.zero;
        }
    }
}