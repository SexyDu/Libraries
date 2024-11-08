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

        /// <summary>
        /// 초기 설정
        /// </summary>
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
                // 데이터의 터치수와 실제 입력받은 터치수가 같으면 터치 프로세스 진행
                if (data.Count == touches.Count)
                    ProcessTouch();
                // 다른 경우 새로운 터치 수에 따른 설정 진행
                else
                    SettingTouch();

                yield return null;
            } while (true);
        }

        /// <summary>
        /// 터치 초기값 설정
        /// </summary>
        private void SettingTouch()
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
        [SerializeField] private HandleType[] handleTypes; // 사용할 핸들 타입

        // 핸들 인터페이스
        private ITransformHandle[] handles = null;

        /// <summary>
        /// 핸들 초기 설정
        /// </summary>
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

        /// <summary>
        /// 핸들 타입에 맞는 핸들 생성 및 인터페이스 반환
        /// </summary>
        /// <param name="type">핸들 타입</param>
        /// <returns>생성된 핸들 인터페이스</returns>
        /// <exception cref="NotSupportedException">타입이 없는 경우의 Exception</exception>
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

        /// <summary>
        /// 전체 핸들 설정
        /// </summary>
        private void SettingHandles()
        {
            for (int i = 0; i < handles.Length; i++)
            {
                handles[i].Setting();
            }
        }

        /// <summary>
        /// 전체 핸들 업무 수행
        /// </summary>
        /// <returns>업무 수행에 따른 위치 이동값</returns>
        private Vector2 ProcessHandles()
        {
            Vector2 deltaPosition = Vector2.zero;
            for (int i = 0; i < handles.Length; i++)
            {
                deltaPosition += handles[i].Process();
            }

            return deltaPosition;
        }
        #endregion


        [Serializable]
        private enum HandleType : byte
        {
            Unknown = 0,
            Position = 1, // 위치 이동
            Scale = 2, // 크기 변경
            Angle = 3, // 각도 조절
        }
    }
}