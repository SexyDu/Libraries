using UnityEngine;

namespace SexyDu
{
    namespace Touch
    {
        /// <summary>
        /// 화면 터치 정보 클래스
        /// </summary>
        public class TouchTabInformation
        {
            private int fingerId = int.MinValue;
            private Vector2 touchPos;
            private float touchTime;

            public int FingerId => fingerId;
            public Vector2 TouchPos => touchPos;
            public float TouchTime => touchTime;
            
            public bool IsMouse
            {
                get
                {
                    return fingerId == TouchCenter.MouseIdLeft || fingerId == TouchCenter.MouseIdRight;
                }
            }

            public TouchTabInformation(int fingerId)
            {
                this.fingerId = fingerId;
                touchPos = GetCurrentPosition();
                touchTime = Time.time;
            }

            public TouchTabInformation(int fingerId, Vector2 touchPos, float touchTime)
            {
                this.fingerId = fingerId;
                this.touchPos = touchPos;
                this.touchTime = touchTime;
            }

            private Vector2 GetCurrentPosition()
            {
                if (IsMouse)
                {
                    return Input.mousePosition;
                }
                else
                {
                    for (int i = 0; i < Input.touches.Length; i++)
                    {
                        if (Input.touches[i].fingerId.Equals(fingerId))
                            return Input.touches[i].position;
                    }
                }

                return TouchCenter.Config.InvalidTouchPosition;
            }
        }
    }
}