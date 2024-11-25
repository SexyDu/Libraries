namespace SexyDu.Tool
{
    public sealed class OnFrameContainer : OnFrameSubject, IOnFrameContainer
    {
        ~OnFrameContainer()
        {
            UnityEngine.Debug.Log("OnFrameContainer 소멸");
        }

        public bool Has(IOnFrameTarget target)
        {
            return targets.Contains(target);
        }

        public void Dispose()
        {
            Clear();
        }
    }
}