namespace SexyDu.Tool
{
    /// <summary>
    /// 매 프레임 동작해야하는 작업을 모아 수행하는 컨테이너 인터페이스
    /// </summary>
    public interface IOnFrameContainer
    {
        /// <summary>
        /// 프래임 동작 대상 등록
        /// </summary>
        public void Bind(IOnFrameTarget target);
        /// <summary>
        /// 프래임 동작 대상 해제
        /// </summary>
        public void Unbind(IOnFrameTarget target);
    }
}