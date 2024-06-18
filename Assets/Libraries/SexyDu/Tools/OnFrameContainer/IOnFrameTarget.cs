namespace SexyDu.Tool
{
    // 매 프레임 동작해야하는 작업하는 대상 인터페이스
    public interface IOnFrameTarget
    {
        /// <summary>
        /// 프레임 수행 함수
        /// </summary>
        public void OnFrame();
    }
}