#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// [에디터 전용] ContainerDocker 에디터 표시용 오브젝트
    /// </summary>
    public class ContainerDockerOnEditor : MonoBehaviour
    {
        /// <summary>
        /// 오브젝트 생성
        /// </summary>
        public static ContainerDockerOnEditor Create()
        {
            GameObject obj = new GameObject();

            obj.name = "ContainerDockerOnEditor";
            DontDestroyOnLoad(obj);

            return obj.AddComponent<ContainerDockerOnEditor>();
        }

        // 현재 도킹된 오브젝트 리스트
        [SerializeField] private List<DockableInformation> docking = new List<DockableInformation>();
        // 도킹 해제된 오브젝트 리스트
        [SerializeField] private List<DockableInformation> undocked = new List<DockableInformation>();

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
        /// <returns></returns>
        private StackFrame GetCallFrame()
        {
            StackTrace stackTrace = new StackTrace(true);

            if (stackTrace.FrameCount < 4)
                return null;
            else
                return stackTrace.GetFrame(3); // 여기 시점 3번째가 실제 호출부
        }

        #region Structs
        /// <summary>
        /// 컨테이너 도킹 관련 이력
        /// </summary>
        [Serializable]
        public struct DockingHistory
        {
            // 표시 시간 포맷
            private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

            // 시점
            [SerializeField] private string dateTime;

            [Header("Method")]
            // 메서드 정보
            [SerializeField][TextArea] private string methodInformation;

            [Header("File")]
            // 스크립트 파일 에셋 경로
            [SerializeField][TextArea] private string scriptAssetPath;
            // 스크립트 줄
            [SerializeField] private int lineInScript;

            public DockingHistory(StackFrame frame)
            {
                this.dateTime = DateTime.Now.ToString(TimeFormat);

                if (frame != null)
                {
                    string filePath = frame.GetFileName();
                    this.scriptAssetPath = Application.dataPath.Length < filePath.Length ? filePath.Substring(Application.dataPath.Length) : filePath;
                    this.lineInScript = frame.GetFileLineNumber();
                    if (frame.HasMethod())
                        this.methodInformation = GetMethodInformation(frame.GetMethod());
                    else
                        this.methodInformation = string.Empty;
                }
                else
                {
                    this.scriptAssetPath = string.Empty;
                    this.lineInScript = 0;

                    this.methodInformation = string.Empty;
                }
            }

            /// <summary>
            /// MethodBase를 기반으로 Method 정보 문자열 반환
            /// </summary>
            private static string GetMethodInformation(MethodBase methodBase)
            {
                StringBuilder sb = new StringBuilder(methodBase.DeclaringType.Name);
                sb.AppendFormat(".{0} (", methodBase.Name);
                ParameterInfo[] parameters = methodBase.GetParameters();
                if (parameters != null && parameters.Length > 0)
                {
                    sb.Append(parameters[0].ParameterType);

                    for (int i = 1; i < parameters.Length; i++)
                    {
                        sb.AppendFormat(", {0}", parameters[i].ParameterType);
                    }
                }
                sb.AppendLine(")");

                sb.AppendFormat("(namespace {0})", methodBase.DeclaringType.Namespace);

                return sb.ToString();
            }
        }

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
            [SerializeField] private DockingHistory docked;
            // 컨테이너 도킹 해제 이력
            [SerializeField] private DockingHistory undocked;

            public DockableInformation(Type keyType, Type valueType)
            {
                this.keyType = keyType;
                this.valueType = valueType;

                this.key = this.keyType.Name;
                this.value = this.valueType.Name;

                this.docked = new DockingHistory();
                this.undocked = new DockingHistory();
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
                this.docked = new DockingHistory(stackFrame);
            }

            /// <summary>
            /// 컨테이너 도킹 해제 이력 처리
            /// </summary>
            public void Undock(StackFrame stackFrame = null)
            {
                this.undocked = new DockingHistory(stackFrame);
            }
        }
        #endregion
    }
}
#endif