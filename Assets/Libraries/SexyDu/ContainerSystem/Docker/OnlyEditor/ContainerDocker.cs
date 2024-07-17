#if UNITY_EDITOR
using UnityEngine;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// [에디터 전용] ContainerDocker Editor 관리
    /// </summary>
    public static partial class ContainerDocker
    {
        /// <summary>
        /// ContainerDocker 에디터 표출 오브젝트
        /// </summary>
        private static ContainerDockerOnEditor onEditor = null;

        /// <summary>
        /// ContainerDockerOnEditor 생성
        /// </summary>
        private static void CreateOnEditor()
        {
            if (onEditor == null)
            {
                onEditor = ContainerDockerOnEditor.Create();
                Debug.LogFormat("<color=yellow>[에디터 전용]</color> ContainerDocker 정보를 게임오브젝트 '{0} (DontDestroyOnLoad)'에서 확인할 수 있습니다.", onEditor.name);
            }
#if false
            else
                Debug.LogWarningFormat("이미 생성된 ContainerDockerOnEditor가 존재합니다, In hierarchy '{0}'", onEditor.name);
#endif
        }
    }
}
#endif