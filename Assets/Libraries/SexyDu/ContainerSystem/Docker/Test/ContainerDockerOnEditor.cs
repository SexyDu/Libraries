using System;
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
            dock.Dock();

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
                    undock.Undock();

                    docking.RemoveAt(i);
                    undocked.Add(undock);
                    break;
                }
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
        [SerializeField] private string dockedTime;
        [SerializeField] private string undockedTime;

        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        private string CurrentTimeString => DateTime.Now.ToString(TimeFormat);

        public DockableObject(Type keyType, Type valueType)
        {
            this.keyType = keyType;
            this.valueType = valueType;

            this.key = this.keyType.Name;
            this.value = this.valueType.Name;
            this.dockedTime = string.Empty;
            this.undockedTime = string.Empty;
        }

        public bool Equals(Type type)
        {
            return keyType.Equals(type);
        }

        public void Dock()
        {
            this.dockedTime = CurrentTimeString;
        }

        public void Undock()
        {
            this.undockedTime = CurrentTimeString;
        }
    }
}