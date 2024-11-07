using UnityEngine;

namespace SexyDu.Touch
{
    public interface ITransformHandler
    {
        public Transform Target { get; }
        public MultiTouchData Data { get; }
    }
}