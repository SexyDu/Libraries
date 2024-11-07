using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public class TransformHandler : MultiTouchBase, ITransformHandler
    {
        [SerializeField] private bool onAwakeInit = true;
        
        protected virtual void Awake()
        {
            if (onAwakeInit)
                Initialize();
        }

        public virtual void Initialize()
        {
            InitializeHandles();
        }

        #region Transform
        [Header("TransformHandler")]
        // 터치 transform 대상
        [SerializeField] protected Transform target;
        /// <summary>
        /// 터치 transform 대상 Property
        /// : IMultiTouchBody
        /// </summary>
        public Transform Target => target;
        // 대상 위치 값
        private Vector3 position = Vector2.zero;
        /// <summary>
        /// 대상 위치 이동
        /// </summary>
        protected virtual void Translate(Vector2 delta)
        {
            SetPosition(position.x + delta.x, position.y + delta.y);
        }
        /// <summary>
        /// 대상 위치 설정
        /// </summary>
        protected virtual void SetPosition(float x, float y)
        {
            position.x = x;
            position.y = y;
            target.position = position;
        }
        #endregion

        #region Data
        /// <summary>
        /// 멀티 터치 데이터 Property
        /// : IMultiTouchBody
        /// </summary>
        public MultiTouchData Data => data;
        #endregion


        protected override IEnumerator CoRoutine()
        {
            do
            {
                if (data.Count == touches.Count)
                    ProcessTouch();
                else
                    InitializeTouch();

                yield return null;
            } while (true);
        }

        /// <summary>
        /// 터치 초기값 설정
        /// </summary>
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
                SettingHandles();
            }
        }

        /// <summary>
        /// 터치 진행
        /// </summary>
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
                Translate(ProcessHandles());
            }
        }

        #region Handles
        [Header("Handles")]
        [SerializeField] private HandleType[] handleTypes;

        private ITransformHandle[] handles = null;

        private void InitializeHandles()
        {
            List<ITransformHandle> list = new List<ITransformHandle>();
            
            for (int i = 0; i < handleTypes.Length; i++)
            {
                try
                {
                    list.Add(CreateHandle(handleTypes[i]).SetBody(this));
                }
                catch (NotSupportedException e)
                {
                    Debug.LogFormat("[Transform Handle 추가 실패] {0}", e.Message);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            handles = list.ToArray();
        }

        private ITransformHandle CreateHandle(HandleType type)
        {
            switch (type)
            {
                case HandleType.Position:
                    return new TransformPositionHandle();
                case HandleType.Scale:
                    return new TransformScaleHandle();
                case HandleType.Angle:
                    return new TransformAngleHandle();
                default:
                    throw new NotSupportedException($"해당 HandleType({type})에 맞는 클래스가 없습니다.");
            }
        }

        private void SettingHandles()
        {
            for (int i = 0; i < handles.Length; i++)
            {
                handles[i].Setting();
            }
        }

        private Vector2 ProcessHandles()
        {
            Vector2 deltaPosition = Vector2.zero;
            for (int i = 0; i < handles.Length; i++)
            {
                handles[i].Process();
                deltaPosition += handles[i].DeltaPositionAfterProcess;
            }

            return deltaPosition;
        }
        #endregion


        [System.Serializable]
        private enum HandleType
        {
            Unknown = 0,
            Position = 1,
            Scale = 2,
            Angle = 3,
        }
    }
}