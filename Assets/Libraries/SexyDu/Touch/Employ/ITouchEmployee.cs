using UnityEngine;

namespace SexyDu.Touch
{
    public interface ITouchEmployee
    {
        public void SetEmpoloyer(ITouchEmployer employer);

        public void Detect(int fingerId);

        public void Detect(int fingerId, Vector2 pos);

        public void Detect(int fingerId, Vector2 pos, float time);

        public void Disappear();

        public void Cancel();
    }
}