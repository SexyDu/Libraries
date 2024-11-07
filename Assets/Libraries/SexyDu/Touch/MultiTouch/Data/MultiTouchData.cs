using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 다중 터치 데이터
    /// </summary>
    public struct MultiTouchData
    {
        // 터치 배열
        private TouchData[] touches;
        // 터치 수
        private int count;
        // 나눗셈을 곱셉으로 계산하기 위한 터치수 값
        private float countForMult;

        // 센터 위치값
        public Vector2 center
        {
            private set;
            get;
        }

        // [Property] 터치 배열
        public TouchData[] Touches => touches;

        // [Property] 터치 수
        public int Count => count;
        // [Property] 나눗셈을 곱셉으로 계산하기 위한 터치수 값 프로퍼티
        public float CountForMultiple => countForMult;

        // 터치 유효 여부
        public bool IsValid
        {
            get
            {
                for (int i = 0; i < touches.Length; i++)
                {
                    if (!touches[i].IsValid)
                        return false;
                }

                return false;
            }
        }

        /// <summary>
        /// 터치 수 설정
        /// </summary>
        public void Set(int count)
        {
            this.count = count;
            countForMult = 1f / (float)count;
        }
        /// <summary>
        /// 터치 배열 설정
        /// </summary>
        /// <param name="touches"></param>
        public void Set(TouchData[] touches)
        {
            // 터치 설정
            this.touches = touches;

            // 터치 센터 설정
            Vector2 sum = Vector2.zero;
            for (int i = 0; i < this.touches.Length; i++)
            {
                sum += touches[i].position;
            }
            center = sum * countForMult;
        }

        // 터치 클리어
        public void Clear()
        {
            count = 0;
            countForMult = 0f;
            touches = null;
        }

        /// <summary>
        /// 유효하지 않은 터치의 fingerId 배열 반환
        /// </summary>
        /// <returns></returns>
        public int[] GetInvalidFingerIds()
        {
            List<int> fingerIds = null;

            for (int i = 0; i < touches.Length; i++)
            {
                if (!touches[i].IsValid)
                {
                    if (fingerIds == null)
                        fingerIds = new List<int>(touches[i].fingerId);

                    fingerIds.Add(touches[i].fingerId);
                }
            }

            if (fingerIds == null)
                return null;
            else
                return fingerIds.ToArray();
        }
    }
}