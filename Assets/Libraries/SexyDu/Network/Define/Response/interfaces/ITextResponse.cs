namespace SexyDu.Network
{
    /// <summary>
    /// Text 수신 데이터 인터페이스
    /// </summary>
    public interface ITextResponse : IResponse
    {
        // 수신 데이터
        public string text { get; }
    }
}