using UnityEngine;

namespace SexyDu.ContainerSystem
{
    public static partial class ContainerDocker
    {
        private static ContainerDockerOnEditor onEditor = null;

        public static void CreateOnEditor()
        {
            if (onEditor == null)
                onEditor = ContainerDockerOnEditor.Create();
            else
                Debug.LogWarningFormat("이미 생성된 ContainerDockerOnEditor가 존재합니다, In hierarchy '{0}'", onEditor.name);
        }
    }
}