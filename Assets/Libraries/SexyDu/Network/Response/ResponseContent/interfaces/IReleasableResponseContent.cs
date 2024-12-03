namespace SexyDu.Network
{
    /// <summary>
    /// Release 함수를 가지는 수신 컨텐츠 인터페이스
    ///  * 별도 삭제 과정을 거칠 필요성이 있는 경우 사용 (ex. Sprite)
    /// </summary>
    public interface IReleasableResponseContent : IReleasable { }
}