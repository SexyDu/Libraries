using UnityEngine;
using UnityEditor;

namespace SexyDu.Network.Editor
{
    public abstract class BaseTester : ScriptableObject, IClearable, IReleasable
    {
        /// <summary>
        /// 초기화 설정
        /// </summary>
        /// <param name="window">관리 EditorWindow</param>
        public virtual void Initialize(EditorWindow window)
        {
            parentWindow = window;
        }

        /// <summary>
        /// Window 종료에 따른 해제
        /// </summary>
        public virtual void Release()
        {
            parentWindow = null;

            ClearNetworker();
        }

        public virtual void Clear()
        {
            ClearNetworker();
        }

        /// <summary>
        /// EditorGUI 그리기
        /// </summary>
        public abstract void OnEditorGUI();

        #region EditorWindow
        // 관리 EditorWindow
        private EditorWindow parentWindow = null;
        // EditorWindow 갱신
        protected void Repaint() => parentWindow?.Repaint();
        #endregion

        #region Network
        // 수행 네트워커
        private INetworker networker = null;
        // 네트워크 수행 여부
        public bool IsWorking => networker != null;
        /// <summary>
        /// 수행 네트워크 설정
        /// </summary>
        /// <param name="networker"></param>
        protected void SetNetworker(INetworker networker)
        {
            this.networker = networker;
        }
        /// <summary>
        /// 수행 네트워크 클리어
        /// </summary>
        protected void ClearNetworker()
        {
            SetNetworker(null);
        }
        #endregion

        #region Temporary Url
        /// <summary>
        /// 지정된 임시 Url로 설정
        /// </summary>
        protected abstract void SetTemporaryUrl();
        /// <summary>
        /// 지정된 임시 Url로 설정 버튼 표시
        /// </summary>
        protected void ButtonTemporaryUrl()
        {
            if (GUILayout.Button("임시 Url", GUILayout.Width(67)))
                SetTemporaryUrl();
        }
        #endregion
    }
}