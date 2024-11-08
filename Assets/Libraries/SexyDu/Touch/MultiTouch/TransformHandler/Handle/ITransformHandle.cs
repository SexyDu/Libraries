using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티터치 Transform 업무수행 핸들 인터페이스
    /// </summary>
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
        /// <returns>처리에 따른 위치 이동값(delta position)</returns>
        public Vector2 Process();
    }
}