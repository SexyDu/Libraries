namespace SexyDu.Touch
{
    public interface ITouchTarget
    {.
        public void ReceiveTouch(int fingerId);

        public void ClearTouch();
    }
}