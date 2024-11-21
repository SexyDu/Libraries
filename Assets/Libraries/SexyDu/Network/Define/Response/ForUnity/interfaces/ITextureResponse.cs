using UnityEngine;

namespace SexyDu.Network
{
    /// <summary>
    /// Texture2D 수신 데이터 인터페이스
    /// </summary>
    public interface ITextureResponse : IResponse
    {
        // 수신 데이터
        public Texture2D tex { get; }
    }
}
