using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class TouchEmployee : MonoBehaviour, ITouchEmployee
    {
        protected ITouchEmployer employer = null;

        public virtual void SetEmpoloyer(ITouchEmployer employer)
        {
            this.employer = employer;
        }

        public abstract void Detect(int fingerId);

        public abstract void Detect(int fingerId, Vector2 pos);

        public abstract void Detect(int fingerId, Vector2 pos, float time);

        public abstract void Disappear();

        public abstract void Cancel();

        protected void Report()
        {
            employer?.ReceiveReport();
        }
    }
}