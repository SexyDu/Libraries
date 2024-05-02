using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.PageViewSystem
{
    public class PageContentsHandler : IPageContentRemoveReceiver, ISubjectContentsCount
    {
        // 한번에 살아있을 수 있는 최대 PageView 수
        private const int MaxAlivePageViewCount = 5;

        private List<PageContent> contents = new List<PageContent>();
        private int lastIndex { get { return Count - 1; } }

        public PageContent Current
        {
            get
            {
                if (Count.Equals(0))
                    return null;
                else
                    return contents[lastIndex];
            }
        }

        #region Subject - ISubjectContentsCount
        private List<IObserverContentsCount> observers = new List<IObserverContentsCount>();

        public int Count { get { return contents.Count; } }
        public void Subscribe(IObserverContentsCount observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }
        public void Unsubscribe(IObserverContentsCount observer)
        {
            observers.Remove(observer);
        }

        private void NotifyContentsCount()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnContentsCountChanged(Count);
            }
        }
        #endregion


        public void Add(PageContent content)
        {
            if (content != null)
            {
                InactiveLastestPageView();

                content.SetRemoveReceiverInterface(this);
                content.LoadPageView();

                contents.Add(content);

                ActiveLastestPageView();

                DestroyLegacyPageView();

                NotifyContentsCount();
            }
        }

        public void Add(string resourcesPath, object arg = null, Transform parent = null)
        {
            Add(Create(resourcesPath, arg, parent));
        }

        private PageContent Create(string resourcesPath, object arg, Transform parent)
        {
            return new PageContent(resourcesPath, arg, parent);
        }

        public void Remove(PageContent content)
        {
            contents.Remove(content);

            content.Dispose();

            ActiveLastestPageView();

            ReloadLagecyPageView();

            NotifyContentsCount();
        }

        public void RemoveLastest()
        {
            if (Count > 0)
                Remove(contents[lastIndex]);
        }

        public void RemoveAll()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                contents[i].Dispose();
            }

            contents.Clear();

            NotifyContentsCount();
        }

        #region Handling PageView
        private void ActiveLastestPageView()
        {
            if (Count > 0)
                contents[lastIndex].SetPageViewActive(enable);
        }

        private void InactiveLastestPageView()
        {
            if (Count > 0)
                contents[lastIndex].SetPageViewActive(false);
        }

        private void DestroyLegacyPageView()
        {
            int index = Count - 1 - MaxAlivePageViewCount;

            if (index >= 0)
                contents[index].DestoryPageView();
        }

        private void ReloadLagecyPageView()
        {
            int index = Count - MaxAlivePageViewCount;

            if (index >= 0 && !contents[index].HasPageView)
                contents[index].LoadPageView(false);
        }
        #endregion

        #region Enable
        private bool enable = true;
        public bool Enable
        {
            get { return enable; }

            set
            {
                enable = value;

                ActiveLastestPageView();
            }
        }
        #endregion
    }
}