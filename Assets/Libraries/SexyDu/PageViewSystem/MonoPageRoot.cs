using UnityEngine;

namespace SexyDu.PageViewSystem
{
    public abstract class MonoPageRoot : MonoBehaviour
    {
        [SerializeField] private Transform pageViewParent;

        private PageContentsHandler contentsHandler = new PageContentsHandler();
        public PageContentsHandler ContentsHandler { get { return contentsHandler; } }

        public virtual void RegisterPageRoot()
        {
            PageViewLoader.Instance.SetPageRoot(this);
        }

        public virtual void UnregisterPageRoot()
        {
            PageViewLoader.Instance.ReleasePageRoot(this);
        }

        public void Add(string resourcesPath, object arg = null)
        {
            ContentsHandler.Add(resourcesPath, arg, pageViewParent);
        }
    }
}