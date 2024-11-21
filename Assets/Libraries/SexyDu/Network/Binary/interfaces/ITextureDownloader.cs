using System;

namespace SexyDu.Network
{
    /// <summary>
    /// Texture(2D) 다운로드 작업자 인터페이스
    /// </summary>
    public interface ITextureDownloader : ITextureSubject
    {
        public ITextureDownloader Request(IBinaryReceipt receipt);
    }
    /// <summary>
    /// Texture(2D) 다운로드 콜백 옵저버 서브젝트 인터페이스
    /// </summary>
    public interface ITextureSubject
    {
        public ITextureSubject Subscribe(Action<ITextureResponse> callback);
    }
}