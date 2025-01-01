using System;
using UnityEngine;

namespace SexyDu.UI
{
    /// <summary>
    /// 버튼 상호작용
    /// </summary>
    public abstract class ButtonInteract : MonoBehaviour, IButtonInteract
    {
        public abstract void OnButtonPress();

        public abstract void OnButtonUp();

        #if UNITY_EDITOR
        /// <summary>
        /// 기본 설정 구성
        /// </summary>
        public virtual void ConstructDefaultSetting()
        {
            throw new NotSupportedException("여기선 기본 설정 구성을 지원하지 않습니다.");
        }
        #endif
    }
}