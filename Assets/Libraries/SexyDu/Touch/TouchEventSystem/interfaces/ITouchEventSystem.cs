using UnityEngine;

namespace SexyDu.Touch
{
    public interface ITouchEventSystem
    {
        public void Subscribe(ITouchEventReceiver receiver);
        public void Unsubscribe(ITouchEventReceiver receiver);
        public void ClearSubscription();
    }

    public interface ITouchEventReceiver
    {
        void OnTouchBegin(UnityEngine.Touch touch);
        void OnMouseBegin(int mouseId, Vector2 position);
    }
}