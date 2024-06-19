using System;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// Singleton 형식 오브젝트를 적재할 컨테이너 ADT(추상 데이터 타입) 인터페이스
    /// * ISingleBaggage 인터페이스만을 다루기 때문에 작업 복잡도는 약간 올라가지만
    ///   확실한 제약을 두어 추후 유지보수나 코드 분석에 용이성 향상
    /// </summary>
    public interface ISingleContainer : IDockable
    {
        /// <summary>
        /// 오브젝트 적재
        /// </summary>
        public void Bind<T>(T data) where T : ISingleBaggage;

        /// <summary>
        /// 오브젝트 방출
        /// </summary>
        public void Unbind<T>() where T : ISingleBaggage;

        /// <summary>
        /// 오브젝트 반환
        /// </summary>
        public T Get<T>() where T : ISingleBaggage;

        /// <summary>
        /// 오브젝트 존재 여부
        /// </summary>
        public bool Has<T>() where T : ISingleBaggage;

        /// <summary>
        /// 오브젝트 존재 여부
        /// </summary>
        public bool Has(Type key);
    }

    /// <summary>
    /// ISingleContainer에 적재될 오브젝트 인터페이스
    /// </summary>
    public interface ISingleBaggage
    {

    }
}