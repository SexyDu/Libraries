

using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 관성 동작 인터페이스
    /// </summary>
    public interface IInertiaProcessor
    {
        /// <summary>
        /// 관성 대상 설정
        /// </summary>
        public IInertiaProcessor Set(IInertiaTarget target);

        /// <summary>
        /// 코루틴 워커 설정
        /// </summary>
        public IInertiaProcessor Set(MonoBehaviour worker);

        /// <summary>
        /// 관성 동작 실행
        /// </summary>
        /// <param name="inertiaForce">관성력</param>
        /// <param name="deltaTime">관성력 작용 시간(deltaTime)</param>
        public void Run(Vector2 inertiaForce, float deltaTime);

        /// <summary>
        /// 관성 동작 종료
        /// </summary>
        public void Stop();
    }
}