using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티터치 Transform 관리자 인터페이스
    /// </summary>
    public interface ITransformHandler
    {
        /// <summary>
        /// Transform 대상
        /// </summary>
        public Transform Target { get; }
        /// <summary>
        /// 실시간 터치 데이터
        /// </summary>
        public MultiTouchData Data { get; }
    }
}