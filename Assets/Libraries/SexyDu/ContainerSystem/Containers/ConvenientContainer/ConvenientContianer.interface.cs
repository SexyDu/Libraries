using System;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// Singleton 형식 오브젝트를 적재할 컨테이너 ADT(추상 데이터 타입) 인터페이스
    /// * 모든 형식의 오브젝트를 다루기 때문에 작업 시 제약설정을 하지 않아도 되기 때문에 편리하다.
    ///   하지만 제약이 없기 때문에 코드 분석이 약간 불편하질 수 있다.
    /// </summary>
    public interface IConvenientContainer : IDockable
    {
        /// <summary>
        /// 오브젝트 적재
        /// </summary>
        public void Bind<T>(T data);

        /// <summary>
        /// 오브젝트 방출
        /// </summary>
        public void Unbind<T>();

        /// <summary>
        /// 오브젝트 반환
        /// </summary>
        public T Get<T>();

        /// <summary>
        /// 오브젝트 존재 여부
        /// </summary>
        public bool Has<T>();

        /// <summary>
        /// 오브젝트 존재 여부
        /// </summary>
        public bool Has(Type key);
    }
}