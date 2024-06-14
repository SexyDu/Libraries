namespace SexyDu.Touch
{
    public interface ITouchTarget
    {
        public void AddTouch(int fingerId);

        public void ClearTouch();
    }
}