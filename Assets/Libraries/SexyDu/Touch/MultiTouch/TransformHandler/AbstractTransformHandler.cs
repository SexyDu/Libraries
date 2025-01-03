using System.Collections;
using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class AbstractTransformHandler : MultiTouchBase, ITransformHandler
    {
        #region Transform
        // 터치 transform 대상
        [SerializeField] protected Transform target;
        /// <summary>
        /// 터치 transform 대상 Property
        /// : ITransformHandler
        /// </summary>
        public Transform Target => target;
        // 대상 위치 값
        protected Vector3 position = Vector2.zero;
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
        /// : ITransformHandler
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
        protected virtual void SettingTouch()
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
        protected virtual void ProcessTouch()
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
        /// <summary>
        /// 전체 핸들 설정
        /// </summary>
        protected abstract void SettingHandles();

        /// <summary>
        /// 전체 핸들 업무 수행
        /// </summary>
        /// <returns>업무 수행에 따른 위치 이동값</returns>
        protected abstract Vector2 ProcessHandles();
        #endregion
    }
}