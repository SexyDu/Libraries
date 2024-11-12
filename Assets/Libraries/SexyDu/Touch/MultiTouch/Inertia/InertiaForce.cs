using UnityEngine;

namespace SexyDu.Touch
{
    public struct InertialForce : IClearable
    {
        public Vector2 deltaPosition
        {
            get;
            private set;
        }

        public float deltaTime
        {
            get;
            private set;
        }

        public bool Available
        {
            get
            {
                return deltaPosition != Vector2.zero && deltaTime > 0f;
            }
        }

        public void Set(Vector2 deltaPosition, float deltaTime)
        {
            this.deltaPosition = deltaPosition;
            this.deltaTime = deltaTime;
        }

        public Vector2 GetDeltaPosition(float baseFrameSeconds)
        {
            return deltaPosition * baseFrameSeconds / deltaTime;
        }

        public void Clear()
        {
            Set(Vector2.zero, 0f);
        }
    }
}
