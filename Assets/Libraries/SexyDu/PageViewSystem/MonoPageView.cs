using System;
using UnityEngine;

namespace SexyDu.PageViewSystem
{
    public abstract class MonoPageView : MonoBehaviour
    {
        /// <summary>
        /// 초기 설정 함수
        /// </summary>
        public abstract MonoPageView Initialize(object arg = null);

        /// <summary>
        /// 클리어(메모리 해제 등) 함수
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// PageView 파괴
        /// </summary>
        public virtual void Destroy()
        {
            Clear();

            GameObject.Destroy(GameObjectCache);
        }

        // PageView 종료 이벤트
        public Action OnClosed { set; private get; }
        /// <summary>
        /// PageView 종료
        /// </summary>
        public virtual void Close()
        {
            Destroy();

            if (OnClosed != null)
                OnClosed();
        }

        #region ObjectCaches
        [Header("Object Caches")]
        [SerializeField] private GameObject gameObjectCache;
        protected GameObject GameObjectCache { get { return gameObjectCache; } }

        public void SetActive(bool active)
        {
            GameObjectCache.SetActive(active);
        }
        #endregion
    }
}