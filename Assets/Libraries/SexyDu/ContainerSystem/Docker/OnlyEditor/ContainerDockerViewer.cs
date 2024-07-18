#if UNITY_EDITOR
using UnityEngine;

namespace SexyDu.ContainerSystem.Editor
{
    /// <summary>
    /// [에디터 전용] ContainerDocker 에디터 표시용 오브젝트
    /// </summary>
    public class ContainerDockerViewer : MonoBehaviour
    {
        /// <summary>
        /// 오브젝트 생성
        /// </summary>
        public static ContainerDockerViewer Create()
        {
            GameObject obj = new GameObject();

            obj.name = "ContainerDockerOnEditor";
            DontDestroyOnLoad(obj);

            return obj.AddComponent<ContainerDockerViewer>().Initialize();
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
        private ContainerDockerViewer Initialize()
        {
            history.Initialize();

            return this;
        }

        #region History
        [Header("History")]
        // 도킹 이력 구조체
        [SerializeField] private DockingHistory history = new DockingHistory();
        public DockingHistory History => history;
        #endregion
    }
}
#endif