namespace SexyDu.Network
{
    /// <summary>
    /// byte array 수신 데이터 인터페이스
    /// </summary>
    public interface IBytesResponse : IResponse
    {
        // 수신 데이터
        public byte[] data { get; }
    }
}
