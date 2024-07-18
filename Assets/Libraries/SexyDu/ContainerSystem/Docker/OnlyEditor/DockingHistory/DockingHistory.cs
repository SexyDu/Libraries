#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SexyDu.ContainerSystem.Editor
{
    /// <summary>
    /// 컨테이너 도킹 이력
    /// </summary>
    [Serializable]
    public struct DockingHistory
    {
        // 현재 도킹된 오브젝트 리스트
        [SerializeField] private List<DockableInformation> docking;
        // 도킹 해제된 오브젝트 리스트
        [SerializeField] private List<DockableInformation> undocked;

        /// <summary>
        /// 초기 설정
        /// </summary>
        /// <returns></returns>
        public DockingHistory Initialize()
        {
            docking = new List<DockableInformation>();
            undocked = new List<DockableInformation>();

            return this;
        }

        /// <summary>
        /// 컨테이너 도킹 (에디터용 정보)
        /// </summary>
        public void Dock<T>(T dockable) where T : IDockable
        {
            Type key = typeof(T);
            Type value = dockable.GetType();

            DockableInformation dock = new DockableInformation(key, value);

            dock.Dock(GetCallFrame());

            docking.Add(dock);
        }

        /// <summary>
        /// 컨데이터 도킹 해제 (에디터용 정보)
        /// </summary>
        public void Undock<T>() where T : IDockable
        {
            Type key = typeof(T);

            for (int i = 0; i < docking.Count; i++)
            {
                if (docking[i].Equals(key))
                {
                    DockableInformation undock = docking[i];
                    undock.Undock(GetCallFrame());

                    docking.RemoveAt(i);
                    undocked.Add(undock);
                    break;
                }
            }
        }

        /// <summary>
        /// Dock 또는 Undock 함수를 실행시킨 StackFrame 반환
        /// </summary>
        private StackFrame GetCallFrame()
        {
            StackTrace stackTrace = new StackTrace(true);

            if (stackTrace.FrameCount < 4)
                return null;
            else
                return stackTrace.GetFrame(3); // 여기 시점 3번째가 실제 호출부
        }
    }
}
#endif