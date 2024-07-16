#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.ContainerSystem
{
    public class ContainerDockerOnEditor : MonoBehaviour
    {
        public static ContainerDockerOnEditor Create()
        {
            GameObject obj = new GameObject();

            obj.name = "ContainerDockerOnEditor";
            DontDestroyOnLoad(obj);

            return obj.AddComponent<ContainerDockerOnEditor>();
        }

        [SerializeField] private List<DockableObject> docking = new List<DockableObject>();
        [SerializeField] private List<DockableObject> undocked = new List<DockableObject>();

        public void Dock<T>(T dockable) where T : IDockable
        {
            Type key = typeof(T);
            Type value = dockable.GetType();

            DockableObject dock = new DockableObject(key, value);

            dock.Dock(GetCallFrame());

            docking.Add(dock);
        }

        public void Undock<T>() where T : IDockable
        {
            Type key = typeof(T);

            for (int i = 0; i < docking.Count; i++)
            {
                if (docking[i].Equals(key))
                {
                    DockableObject undock = docking[i];
                    undock.Undock(GetCallFrame());

                    docking.RemoveAt(i);
                    undocked.Add(undock);
                    break;
                }
            }
        }

        private StackFrame GetCallFrame()
        {
            StackTrace stackTrace = new StackTrace(true);

            if (stackTrace.FrameCount < 4)
                return null;
            else
                return stackTrace.GetFrame(3); // 여기 시점 3번째가 실제 호출부
        }
    }

    [Serializable]
    public struct DockingHistory
    {
        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        [SerializeField] private string dateTime;
        [SerializeField][TextArea] private string fileInformation;
        [SerializeField][TextArea] private string methodInformation;

        public DockingHistory(StackFrame frame)
        {
            this.dateTime = DateTime.Now.ToString(TimeFormat);

            if (frame != null)
            {
                this.fileInformation = string.Format("{0}\nln {1}, col {2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber());
                if (frame.HasMethod())
                {
                    MethodBase methodBase = frame.GetMethod();
                    this.methodInformation = string.Format("{0}.{1}\n(namespace {2})", methodBase.DeclaringType.Name, methodBase.Name, methodBase.DeclaringType.Namespace);
                }
                else
                    this.methodInformation = string.Empty;
            }
            else
            {
                this.fileInformation = string.Empty;
                this.methodInformation = string.Empty;
            }
        }
    }

    [Serializable]
    public struct DockableObject
    {
        private Type keyType;
        private Type valueType;

        [SerializeField] private string key;
        [SerializeField] private string value;
        [SerializeField] private DockingHistory docked;
        [SerializeField] private DockingHistory undocked;

        public DockableObject(Type keyType, Type valueType)
        {
            this.keyType = keyType;
            this.valueType = valueType;

            this.key = this.keyType.Name;
            this.value = this.valueType.Name;

            this.docked = new DockingHistory();
            this.undocked = new DockingHistory();
        }

        public bool Equals(Type type)
        {
            return keyType.Equals(type);
        }

        public void Dock(StackFrame stackFrame = null)
        {
            this.docked = new DockingHistory(stackFrame);
        }

        public void Undock(StackFrame stackFrame = null)
        {
            this.undocked = new DockingHistory(stackFrame);
        }
    }
}
#endif