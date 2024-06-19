using System;
using System.Collections.Generic;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// 컨테이너를 연결하여 들고있는 Docker
    /// </summary>
    public static class ContainerDocker // : IDocker
    {
        // 도킹된 컨테이너 Dictionary
        private readonly static Dictionary<Type, IDockable> containers = new Dictionary<Type, IDockable>();

        /// <summary>
        /// 컨테이너 도킹
        /// </summary>
        public static void Dock<T>(T dockable) where T : IDockable
        {
            Type key = typeof(T);
            if (Has(key))
                throw new AlreadyDockedContainerException(key);
            else
                containers.Add(key, dockable);
        }

        /// <summary>
        /// 컨테이너 도킹 해제
        /// </summary>
        public static void Undock<T>() where T : IDockable
        {
            containers.Remove(typeof(T));
        }

        /// <summary>
        /// 컨테이너 반환
        /// </summary>
        public static T Bring<T>() where T : IDockable
        {
            Type key = typeof(T);
            if (Has(key))
                return (T)containers[key];
            else
                return default(T);
        }

        /// <summary>
        /// 컨테이너 존재 여부
        /// </summary>
        public static bool Has<T>() where T : IDockable
        {
            return Has(typeof(T));
        }

        /// <summary>
        /// 컨테이너 존재 여부
        /// </summary>
        public static bool Has(Type key)
        {
            return containers.ContainsKey(key);
        }
    }

    public interface IDockable
    {

    }
}