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
        private readonly Dictionary<Type, ISingleBaggage> baggages = null;

        public SingleContainer()
        {
            baggages = new Dictionary<Type, ISingleBaggage>();
        }

        #region ISingleContainer
        /// <summary>
        /// 오브젝트 적재
        /// : ISingleContainer
        /// </summary>
        public void Bind<T>(T data) where T : ISingleBaggage
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
        public void Unbind<T>() where T : ISingleBaggage
        {
            baggages.Remove(typeof(T));
        }

        /// <summary>
        /// 오브젝트 반환
        /// : ISingleContainer
        /// </summary>
        public T Get<T>() where T : ISingleBaggage
        {
            Type key = typeof(T);
            if (Has(key))
            {
                // 동일한 타입으로의 Unboxing은 비용이 무시해도 될 수준으로 작기 때문에 효율적
                return (T)baggages[key];
            }
            else
                return default(T);
        }

        /// <summary>
        /// 오브젝트 존재 여부
        /// : ISingleContainer
        /// </summary>
        public bool Has<T>() where T : ISingleBaggage
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