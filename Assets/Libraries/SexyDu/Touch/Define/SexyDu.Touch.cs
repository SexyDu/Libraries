using UnityEngine;

namespace SexyDu
{
    namespace Touch
    {
        public struct TouchTabInfo
        {
            private Vector2 touchPos;
            private float touchTime;

            public Vector2 TouchPos { get { return this.touchPos; } }
            public float TouchTime { get { return this.touchTime; } }

            public bool IsNotSet => touchTime.Equals(0f);

            public TouchTabInfo(Vector2 initialPos, float touchTime)
            {
                this.touchPos = initialPos;
                this.touchTime = touchTime;
            }

            public void Clear()
            {
                touchPos = Vector2.zero;
                touchTime = 0;
            }
        }
    }
}