using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class MultiTouchBase : TouchTarget
    {
        public override void AddTouch(int fingerId)
        {
            if (touches.Count < MaxTouchCount)
            {
                touches.Add(fingerId);
            }
        }

        protected virtual void RemoveTouch(int fingerId)
        {
            touches.Remove(fingerId);
        }

        public override void ClearTouch()
        {
            touches.Clear();
        }

        #region Touch
        protected List<int> touches = new List<int>(10);

        public int Count => touches.Count;

        [SerializeField] private int MaxTouchCount = 10;

        protected Vector2 GetTouchPositionAt(int index)
        {
            if (index < 0 || index >= touches.Count)
                return Config.InvalidTouchPosition;
            else
                return GetTouchPosition(touches[index]);
        }

        protected RealtimeMultiTouch GetMultiTouch()
        {
            RealtimeTouch[] realtimeTouches = new RealtimeTouch[Count];
            for (int i = 0; i < realtimeTouches.Length; i++)
            {
                realtimeTouches[i] = new RealtimeTouch(touches[i]);
            }
            return new RealtimeMultiTouch(realtimeTouches);
        }
        #endregion
    }

    public struct RealtimeMultiTouch
    {
        public RealtimeMultiTouch(params RealtimeTouch[] touches)
        {

        }
    }

    public struct RealtimeTouch
    {
        public readonly int fingerId;
        public readonly Vector2 position;

        public bool IsValid => TouchCenter.Config.ValidateTouchPosition(position);

        public RealtimeTouch(int fingerId)
        {
            this.fingerId = fingerId;
            position = TouchCenter.Config.GetTouchPosition(this.fingerId);
        }

        public RealtimeTouch(int fingerId, Vector2 position)
        {
            this.fingerId = fingerId;
            this.position = position;
        }
    }
}