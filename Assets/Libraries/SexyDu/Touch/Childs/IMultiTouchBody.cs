using UnityEngine;

namespace SexyDu.Touch
{
    public interface IMultiTouchBody
    {
        public Transform Target { get; }
        public MultiTouchData Data { get; }
    }
}