using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 추가 핸들을 장착 가능한 트랜스폼 핸들러
    /// </summary>
    public class TransformMountableHandler : TransformHandler
    {
        #region Handles
        /// <summary>
        /// 핸들 초기 설정
        /// : TransformHandler
        /// </summary>
        protected override void InitializeHandles()
        {
            base.InitializeHandles();

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
            
            mountedHandles = list.ToArray();
        }

        /// <summary>
        /// 전체 핸들 설정
        /// : TransformHandler
        /// </summary>
        protected override void SettingHandles()
        {
            base.SettingHandles();

            for (int i = 0; i < mountedHandles.Length; i++)
            {
                mountedHandles[i].Setting();
            }
        }
        /// <summary>
        /// 전체 핸들 업무 수행
        /// : TransformHandler
        /// </summary>
        /// <returns>업무 수행에 따른 위치 이동값</returns>
        protected override Vector2 ProcessHandles()
        {
            Vector2 deltaPosition = base.ProcessHandles();
            
            for (int i = 0; i < mountedHandles.Length; i++)
            {
                deltaPosition += mountedHandles[i].Process();
            }

            return deltaPosition;
        }
        #endregion

        #region Mounted Handles
        // 핸들 인터페이스
        protected ITransformHandle[] mountedHandles = null;

        [Header("Mounted Handles")]
        [SerializeField] private HandleType[] handleTypes; // 사용할 핸들 타입

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
            Angle = 1, // 각도 조절
        }
    }
}