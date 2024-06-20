namespace SexyDu.Tool
{
    /// <summary>
    /// 매 프레임 동작 서브젝트 인터페이스
    /// </summary>
    public interface IOnFrameSubject
    {
        /// <summary>
        /// 프래임 동작 대상 등록
        /// </summary>
        public void Subscribe(IOnFrameTarget target);
        /// <summary>
        /// 프래임 동작 대상 해제
        /// </summary>
        public void Unsubscribe(IOnFrameTarget target);
    }

    /// <summary>
    /// 매 프레임 동작 대상 인터페이스
    /// </summary>
    public interface IOnFrameTarget
    {
        /// <summary>
        /// 프레임 수행 함수
        /// </summary>
        public void OnFrame();
    }
}