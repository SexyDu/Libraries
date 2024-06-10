using UnityEngine;

namespace SexyDu.UI.Unity
{
    public abstract class TouchTargetBasic : MonoBehaviour, ITouchTarget
    {
        public abstract void AddTouch(int fingerId);

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