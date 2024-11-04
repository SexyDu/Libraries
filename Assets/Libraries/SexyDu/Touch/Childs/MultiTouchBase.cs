using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SexyDu.Touch
{
    public /*abstract*/ class MultiTouchBase : TouchTarget
    {
        public override void AddTouch(int fingerId)
        {
            if (touches.Count < MaxTouchCount)
            {
                touches.Add(fingerId);

                if (!IsRunning)
                    RunRoutine();
            }
        }

        protected virtual void RemoveTouch(int fingerId)
        {
            touches.Remove(fingerId);
        }

        protected virtual void RemoveTouches(params int[] fingerIds)
        {
            for (int i = 0; i < fingerIds.Length; i++)
            {
                touches.Remove(fingerIds[i]);
            }

            if (touches.Count == 0)
            {
                StopRoutine();

                data.Clear();
                previous = Vector2.zero;
            }
        }

        public override void ClearTouch()
        {
            touches.Clear();
        }

        #region Transform
        [SerializeField] private Transform target;
        private Vector3 position = Vector2.zero;

        protected virtual void Translate(Vector2 delta)
        {
            SetPosition(position.x + delta.x, position.y + delta.y);
        }

        protected virtual void SetPosition(float x, float y)
        {
            position.x = x;
            position.y = y;
            target.position = position;
        }
        #endregion

        #region TouchData
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

        protected virtual TouchData[] GetTouchDatas()
        {
            TouchData[] datas = new TouchData[touches.Count];
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new TouchData(touches[i]);
            }

            return datas;
        }
        #endregion

        #region TouchRoutine
        private Vector2 previous = Vector2.zero;
        private MultiTouchData data = new MultiTouchData();

        private IEnumerator ieRoutine = null;

        private bool IsRunning => ieRoutine != null;

        private IEnumerator CoRoutine()
        {
            do
            {
                if (data.Count == touches.Count)
                    ProcessTouch();
                else
                    InitializeTouch();

                yield return null;

                Debug.LogFormat("러닝 중");
            } while (true);
        }

        private void InitializeTouch()
        {
            position = target.position;

            data.Set(touches.Count);
            data.Set(GetTouchDatas());

            int[] invalidFingers = data.GetInvalidFingerIds();
            if (invalidFingers != null)
            {
                RemoveTouches(invalidFingers);
            }
            else
            {
                previous = data.Center;
            }
        }

        private void ProcessTouch()
        {
            data.Set(GetTouchDatas());
            int[] invalidFingers = data.GetInvalidFingerIds();
            if (invalidFingers != null)
            {
                RemoveTouches(invalidFingers);
            }
            else
            {
                Vector2 current = data.Center;

                Vector2 delta = Vector2.zero;

                delta += (current - previous) * UPPOP;

                Translate(delta);

                previous = data.Center;
            }
        }

        private void RunRoutine()
        {
            StopRoutine();

            ieRoutine = CoRoutine();
            StartCoroutine(ieRoutine);
        }

        private void StopRoutine()
        {
            if (ieRoutine != null)
            {
                StopCoroutine(ieRoutine);
                ieRoutine = null;
            }
        }
        #endregion
    }

    public struct MultiTouchData
    {
        private TouchData[] touches;
        private int count;
        private float countForMult;

        public int Count => count;

        public bool IsValid
        {
            get
            {
                for (int i = 0; i < touches.Length; i++)
                {
                    if (!touches[i].IsValid)
                        return false;
                }

                return false;
            }
        }

        public void Set(int count)
        {
            this.count = count;
            countForMult = 1f / (float)count;
        }

        public void Set(TouchData[] touches)
        {
            this.touches = touches;
        }

        public void Clear()
        {
            count = 0;
            countForMult = 0f;
        }

        public Vector2 Center
        {
            get
            {
                Vector2 sum = Vector2.zero;

                for (int i = 0; i < touches.Length; i++)
                {
                    sum += touches[i].position;
                }

                return sum *= countForMult;
            }
        }

        public int[] GetInvalidFingerIds()
        {
            List<int> fingerIds = null;

            for (int i = 0; i < touches.Length; i++)
            {
                if (!touches[i].IsValid)
                {
                    if (fingerIds == null)
                        fingerIds = new List<int>(touches[i].fingerId);
                    
                    fingerIds.Add(touches[i].fingerId);
                }
            }

            if (fingerIds == null)
                return null;
            else
                return fingerIds.ToArray();
        }
    }

    public struct TouchData
    {
        public readonly int fingerId;
        public readonly Vector2 position;

        public bool IsValid => TouchCenter.Config.ValidateTouchPosition(position);

        public TouchData(int fingerId)
        {
            this.fingerId = fingerId;
            position = TouchCenter.Config.GetTouchPosition(this.fingerId);
        }

        public TouchData(int fingerId, Vector2 position)
        {
            this.fingerId = fingerId;
            this.position = position;
        }
    }
}