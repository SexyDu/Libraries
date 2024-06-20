namespace SexyDu.Tool
{
    public sealed class OnFrameContainer : OnFrameSubject, IOnFrameContainer
    {
        ~OnFrameContainer()
        {
            UnityEngine.Debug.Log("OnFrameContainer 소멸");
        }

        public void Dispose()
        {
            Clear();
        }
    }
}