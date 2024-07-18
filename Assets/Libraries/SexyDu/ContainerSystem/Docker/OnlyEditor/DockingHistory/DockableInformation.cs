#if UNITY_EDITOR
using System;
using System.Diagnostics;
using UnityEngine;

namespace SexyDu.ContainerSystem.Editor
{
    /// <summary>
    /// 도킹 컨데이터의 정보
    /// </summary>
    [Serializable]
    public struct DockableInformation
    {
        // 도킹 Key
        private Type keyType;
        // 실제 도킹된 오브젝트 타입
        private Type valueType;

        // 도킹 Key 문자열
        [SerializeField] private string key;
        // 실제 도킹된 오브젝트 타입 문자열
        [SerializeField] private string value;
        // 컨테이너 도킹 이력
        [SerializeField] private DockingRecord docked;
        // 컨테이너 도킹 해제 이력
        [SerializeField] private DockingRecord undocked;

        public DockableInformation(Type keyType, Type valueType)
        {
            this.keyType = keyType;
            this.valueType = valueType;

            this.key = this.keyType.Name;
            this.value = this.valueType.Name;

            this.docked = new DockingRecord();
            this.undocked = new DockingRecord();
        }

        /// <summary>
        /// Key 타입 비교
        /// </summary>
        public bool Equals(Type type)
        {
            return keyType.Equals(type);
        }

        /// <summary>
        /// 컨테이너 도킹 이력 처리
        /// </summary>
        public void Dock(StackFrame stackFrame = null)
        {
            this.docked = new DockingRecord(stackFrame);
        }

        /// <summary>
        /// 컨테이너 도킹 해제 이력 처리
        /// </summary>
        public void Undock(StackFrame stackFrame = null)
        {
            this.undocked = new DockingRecord(stackFrame);
        }
    }
}
#endif