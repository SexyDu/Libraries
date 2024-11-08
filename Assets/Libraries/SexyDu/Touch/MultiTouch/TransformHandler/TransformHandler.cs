using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public class TransformHandler : AbstractTransformHandler, ITransformHandler
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
        // 최소 위치값
        [SerializeField] private Vector2 minimumPosition;
        // 최대 위치값
        [SerializeField] private Vector2 maximumPosition;

        /// <summary>
        /// 대상 위치 설정
        /// : AbstractTransformHandler
        /// </summary>
        protected override void SetPosition(float x, float y)
        {
            // x축 제한 위치 보정
            if (x < minimumPosition.x)
                position.x = minimumPosition.x;
            else if (x > maximumPosition.x)
                position.x = maximumPosition.x;
            else
                position.x = x;

            // y축 제한 위치 보정
            if (y < minimumPosition.y)
                position.y = minimumPosition.y;
            else if (y > maximumPosition.y)
                position.y = maximumPosition.y;
            else
                position.y = y;

            target.position = position;
        }

        public TransformHandler SetLimitedMinimumPosition(Vector2 minimum)
        {
            minimumPosition = minimum;

            return this;
        }

        public TransformHandler SetLimitedMaximumPosition(Vector2 maximum)
        {
            maximumPosition = maximum;

            return this;
        }
        #endregion

        #region Handles
        [Header("Handles")]
        [SerializeField] private HandleType[] handleTypes; // 사용할 핸들 타입

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

            RegisterHandles(list.ToArray());
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