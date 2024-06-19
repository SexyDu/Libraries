/// ContainerDocker 클래스 사용 여부 플래그
/// * 사용하지 않는 경우 singleton 독자로 동작한다
#define USE_CONTAINERDOCKER

using System;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// SingleContainer를 Singleton으로 활용하는 (예시목적의) 클래스
    /// </summary>
    public class SingleContainerSingleton
    {
        private static Lazy<ISingleContainer> ins
#if USE_CONTAINERDOCKER
            = new Lazy<ISingleContainer>(() => {
                // 도커에 없다면 할당
                if (!ContainerDocker.Has<ISingleContainer>())
                    ContainerDocker.Dock<ISingleContainer>(new SingleContainer());

                return ContainerDocker.Bring<ISingleContainer>();
            });
#else
            = new Lazy<ISingleContainer>(() => new SingleContainer());
#endif

        public static ISingleContainer Ins => ins.Value;
    }
}