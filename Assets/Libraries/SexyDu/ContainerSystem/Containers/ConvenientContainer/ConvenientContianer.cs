using System;
using System.Collections.Generic;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// IConvenientContainer의 기본 Implement
    /// </summary>
    public class ConvenientContainer : IConvenientContainer
    {
        // 컨테이너 Dictionary
        private readonly Dictionary<Type, object> baggages = null;

        public ConvenientContainer()
        {
            baggages = new Dictionary<Type, object>();
        }

        #region IConvenientContainer
        /// <summary>
        /// 오브젝트 적재
        /// : IConvenientContainer
        /// </summary>
        public void Bind<T>(T data)
        {
            Type key = typeof(T);
            if (Has(key))
                throw new AlreadyBindedBaggageException(key);
            else
                baggages.Add(key, data);
        }

        /// <summary>
        /// 오브젝트 방출
        /// : IConvenientContainer
        /// </summary>
        public void Unbind<T>()
        {
            baggages.Remove(typeof(T));
        }

        /// <summary>
        /// 오브젝트 반환
        /// : IConvenientContainer
        /// </summary>
        public T Get<T>()
        {
            Type key = typeof(T);
            if (Has(key))
                return (T)baggages[key];
            else
                return default(T);
        }

        /// <summary>
        /// 오브젝트 존재 여부
        /// : IConvenientContainer
        /// </summary>
        public bool Has<T>()
        {
            return Has(typeof(T));
        }

        /// <summary>
        /// 오브젝트 존재 여부
        /// : IConvenientContainer
        /// </summary>
        public bool Has(Type key)
        {
            return baggages.ContainsKey(key);
        }
        #endregion
    }
}