using System;
using UnityEngine;

namespace SexyDu.UI.UGUI
{
    /// <summary>
    /// UGUI(RectTransform) SafeArea 설정
    /// </summary>
    public abstract class SafeArea : MonoBehaviour
    {
        [SerializeField] protected bool onEnableSet;
        protected virtual void OnEnable()
        {
            if (onEnableSet)
                Set();
        }

        // SafeArea 타겟
        [SerializeField] protected RectTransform target;
        // 타겟 존재 여부
        protected bool HasTarget => target != null;

        /// <summary>
        /// target에 대한 NullRefernceException 발생
        /// </summary>
        protected void OccurNullTargetException()
        {
            throw new NullReferenceException($"SafeArea({name})에 target이 설정되지 않았습니다.");
        }

        /// <summary>
        /// SafeArea 설정
        /// </summary>
        public abstract void Set();
    }
}