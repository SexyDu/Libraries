using System;
using SexyDu.ContainerSystem;

namespace SexyDu.Tool
{
    /// <summary>
    /// 매 프레임 동작 서브젝트의 Container 인터페이스
    /// * ContainerDocker 연결용
    /// </summary>
    public interface IOnFrameContainer : IOnFrameSubject, IDockable, IDisposable
    {
        
    }
}