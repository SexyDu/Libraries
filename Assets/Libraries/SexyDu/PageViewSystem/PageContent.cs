using System;
using UnityEngine;

namespace SexyDu.PageViewSystem
{
    public class PageContent : IDisposable
    {
        public void Dispose()
        {
            DestoryPageView();

            ReleaseAllInterfaces();
        }

        private readonly string resourcesPath = null; // MonoPageView의 Prefab 경로
        private readonly object arg = null; // MonoPageView 생성 인자
        private readonly Transform parent = null; // MonoPageView의 부모 Transform

        public PageContent(string resourcesPath, object arg, Transform parent)
        {
            this.resourcesPath = resourcesPath;
            this.arg = arg;
            this.parent = parent;
        }

        #region Page
        private MonoPageView pageView = null; // 해당 컨텐츠의 MonoPageView
        public bool HasPageView { get { return pageView != null; } } // PageView 존재 여부

        private void SetPageView(MonoPageView pageView)
        {
            this.pageView = pageView;

            // 페이지 초기 설정 및 페이지종료 이벤트 연결
            this.pageView.Initialize(arg).OnClosed = OnPageViewClosed;
        }

        public void SetPageViewActive(bool active)
        {
            if (HasPageView)
                pageView.SetActive(active);
        }

        private void OnPageViewClosed()
        {
            // 제거 요청 인터페이스가 없는 경우
            if (removeReceiver == null)
                Dispose();
            // 제거 요청 인터페이스가 있는 경우
            else
                removeReceiver.Remove(this);
        }

        public void LoadPageView(bool active = true)
        {
            MonoPageView source = Resources.Load<MonoPageView>(resourcesPath);

            if (source != null)
            {
                MonoPageView clone = MonoBehaviour.Instantiate<MonoPageView>(source, parent);

                SetPageView(clone);

                if (!active)
                    clone.SetActive(false); // or active
            }
            else
            {
                Dispose();

                string message = string.Format("지정된 경로에 MonoPageView 리소스가 존재하지 않습니다.\nResources Path : {0}", resourcesPath);
                throw new NotFoundPageViewException(message);
            }
        }

        public void DestoryPageView()
        {
            if (pageView != null)
            {
                pageView.Destroy();
                pageView = null;
            }
        }
        #endregion

        #region interfaces
        /// <summary>
        /// 전체 인터페이스 해제 함수
        /// </summary>
        private void ReleaseAllInterfaces()
        {
            removeReceiver = null;
        }

        // * PageContent 제거를 요청(접수) 인터페이스
        private IPageContentRemoveReceiver removeReceiver = null;
        public void SetRemoveReceiverInterface(IPageContentRemoveReceiver removeReceiver)
        {
            this.removeReceiver = removeReceiver;
        }
        #endregion
    }
}