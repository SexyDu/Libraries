using System;
using System.Collections.Generic;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// ISingleContainer의 기본 Implement
    /// </summary>
    public class SingleContainer : ISingleContainer
    {
        // 컨테이너 Dictionary
        private readonly Dictionary<Type, IBindable> baggages = null;

        public SingleContainer()
        {
            baggages = new Dictionary<Type, IBindable>();
        }

        #region ISingleContainer
        /// <summary>
        /// 오브젝트 적재
        /// : ISingleContainer
        /// </summary>
        public void Bind<T>(T data) where T : IBindable
        {
            Type key = typeof(T);
            if (Has(key))
                throw new AlreadyBindedBaggageException(key);
            else
                baggages.Add(key, data);
        }

        /// <summary>
        /// 오브젝트 방출
        /// : ISingleContainer
        /// </summary>
        public void Unbind<T>() where T : IBindable
        {
            baggages.Remove(typeof(T));
        }

        /// <summary>
        /// 오브젝트 반환
        /// : ISingleContainer
        /// </summary>
        public T Get<T>() where T : IBindable
        {
            Type key = typeof(T);
            if (Has(key))
                return (T)baggages[key];
            else
                return default(T);
        }

        /// <summary>
        /// 오브젝트 존재 여부
        /// : ISingleContainer
        /// </summary>
        public bool Has<T>() where T : IBindable
        {
            return Has(typeof(T));
        }
        
        /// <summary>
        /// 오브젝트 존재 여부
        /// : ISingleContainer
        /// </summary>
        public bool Has(Type key)
        {
            return baggages.ContainsKey(key);
        }
        #endregion
    }
}