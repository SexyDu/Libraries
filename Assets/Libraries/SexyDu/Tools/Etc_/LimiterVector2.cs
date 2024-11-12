using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SexyDu.Tool
{
    [Serializable]
    public struct LimiterVector2
    {
        [SerializeField] private Vector2 minimum;
        [SerializeField] private Vector2 maximum;

        public Vector2 Correct(Vector2 val)
        {
            // x축 제한 위치 보정
            val.x = CorrectX(val.x);

            // y축 제한 위치 보정
            val.y = CorrectY(val.y);

            return val;
        }

        public float CorrectX(float x)
        {
            if (x < minimum.x)
                return minimum.x;
            else if (x > maximum.x)
                return maximum.x;
            else
                return x;
        }

        public float CorrectY(float y)
        {
            if (y < minimum.y)
                return minimum.y;
            else if (y > maximum.y)
                return maximum.y;
            else
                return y;
        }

        public LimiterVector2 SetMinimum(Vector2 minimum)
        {
            this.minimum = minimum;
            return this;
        }

        public LimiterVector2 SetMaximum(Vector2 maximum)
        {
            this.maximum = maximum;
            return this;
        }
    }
}