using System;
using SexyDu.ContainerSystem;
using SexyDu.Tool;

namespace SexyDu.Tools
{
    /// <summary>
    /// 메인 스레드 디스패처 수행 클래스
    ///  * SexyDu 프레임워크에서 사용하는 메인 스레드 디스패처
    /// </summary>
    public class SexyMainThreadDispatcher : MainThreadDispatcher, IOnFrameTarget
    {
        #region Singleton
        private static readonly Lazy<SexyMainThreadDispatcher> _instance = new Lazy<SexyMainThreadDispatcher>(() =>
        {
            if (!ContainerDocker.Has<IOnFrameContainer>())
                ContainerDocker.Dock<IOnFrameContainer>(new OnFrameContainer());

            var instance = new SexyMainThreadDispatcher();
            return instance;
        });

        public static SexyMainThreadDispatcher Instance => _instance.Value;
        #endregion

        // 메인 스레드 디스패처 수행 여부
        public bool IsRunning => ContainerDocker.Bring<IOnFrameContainer>().Has(this);

        /// <summary>
        /// 메인 스레드 디스패처 수행 시작
        /// </summary>
        public void Run()
        {
            ContainerDocker.Bring<IOnFrameContainer>().Subscribe(this);
        }

        /// <summary>
        /// 메인 스레드 디스패처 수행 중지
        /// </summary>
        public void Stop()
        {
            ContainerDocker.Bring<IOnFrameContainer>().Unsubscribe(this);
        }

        /// <summary>
        /// 프레임 수행 시 호출되는 메서드
        /// </summary>
        public void OnFrame()
        {
            Execute();
        }
    }
}