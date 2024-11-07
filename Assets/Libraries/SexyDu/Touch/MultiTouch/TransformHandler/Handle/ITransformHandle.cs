using UnityEngine;

namespace SexyDu.Touch
{
    public interface ITransformHandle
    {
        /// <summary>
        /// 바디 설정
        /// </summary>
        public ITransformHandle SetBody(ITransformHandler body);

        /// <summary>
        /// 터치 초기 설정
        /// </summary>
        public void Setting();

        /// <summary>
        /// 터치 처리
        /// </summary>
        public void Process();

        /// <summary>
        /// Process처리로 인해 발생한 위치 이동값
        /// </summary>
        public Vector2 DeltaPositionAfterProcess { get; }
    }
}