using System;
using UnityEngine;

namespace SexyDu.Tool
{
    [Serializable]
    public struct LimiterFloat
    {
        [SerializeField] private float minimum;
        [SerializeField] private float maximum;

        public float Correct(float val)
        {
            if (val < minimum)
                return minimum;
            else if (val > maximum)
                return maximum;
            else
                return val;
        }

        public LimiterFloat SetMinimum(float minimum)
        {
            this.minimum = minimum;
            return this;
        }

        public LimiterFloat SetMaximum(float maximum)
        {
            this.maximum = maximum;
            return this;
        }
    }
}