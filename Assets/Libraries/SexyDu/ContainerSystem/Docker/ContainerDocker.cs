using System;
using System.Collections.Generic;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// 컨테이너를 연결하여 들고있는 Docker
    /// </summary>
    public static partial class ContainerDocker
    {
        // 도킹된 컨테이너 Dictionary
        private readonly static Dictionary<Type, IDockable> containers = new Dictionary<Type, IDockable>();

        /// <summary>
        /// 초기 설정
        /// </summary>
        public static void Initialize()
        {
#if UNITY_EDITOR
            // 에디터 모드 생성
            CreateOnEditor();
#endif
        }

        /// <summary>
        /// 컨테이너 도킹
        /// </summary>
        public static void Dock<T>(T dockable) where T : IDockable
        {
            Type key = typeof(T);
            if (Has(key))
                throw new AlreadyDockedContainerException(key);
            else
            {
                containers.Add(key, dockable);

#if UNITY_EDITOR
                onEditor?.Dock<T>(dockable);
#endif
            }
        }

        /// <summary>
        /// 컨테이너 도킹 해제
        /// </summary>
        public static void Undock<T>() where T : IDockable
        {
            containers.Remove(typeof(T));

#if UNITY_EDITOR
            onEditor?.Undock<T>();
#endif
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
        /// Lazy(지연 초기화) 컨테이너 반환
        /// </summary>
        public static T LazyBring<T>() where T : IDockable
        {
            return new Lazy<T>(() => Bring<T>()).Value;
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
}