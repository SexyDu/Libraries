/// ContainerDocker 클래스 사용 여부 플래그
/// * 사용하지 않는 경우 singleton 독자로 동작한다
#define USE_CONTAINERDOCKER

using System;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// ConvenientContainer를 Singleton으로 활용하는 (예시목적의) 클래스
    /// </summary>
    public class ConvenientContainerSingleton
    {
        private static Lazy<IConvenientContainer> ins
#if USE_CONTAINERDOCKER
            = new Lazy<IConvenientContainer>(() => {
                // 도커에 없다면 할당
                if (!ContainerDocker.Has<IConvenientContainer>())
                    ContainerDocker.Dock<IConvenientContainer>(new ConvenientContainer());

                return ContainerDocker.Bring<IConvenientContainer>();
            });
#else
            = new Lazy<IConvenientContainer>(() => new ConvenientContainer());
#endif
        public static IConvenientContainer Ins => ins.Value;
    }
}