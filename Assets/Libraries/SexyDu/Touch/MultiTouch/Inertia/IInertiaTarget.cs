using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 관성 적용 대상 인터페이스
    /// </summary>
    public interface IInertiaTarget
    {
        public void Inertia(Vector2 force);
    }
}