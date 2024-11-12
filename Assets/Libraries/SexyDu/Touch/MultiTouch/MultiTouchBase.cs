using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티 터치 기반 클래스
    /// </summary>
    public abstract class MultiTouchBase : TouchTarget
    {
        /// <summary>
        /// 오브젝트 비활성화 시 이벤트
        /// </summary>
        protected virtual void OnDisable()
        {
            ClearTouch();
        }

        /// <summary>
        /// 터치된 fingerId 수신
        /// </summary>
        /// <param name="fingerId"></param>
        public override void ReceiveTouch(int fingerId)
        {
            if (touches.Count < MaxTouchCount)
            {
                touches.Add(fingerId);

                if (!IsRunning)
                    Run();
            }
        }
        /// <summary>
        /// 터치 제거
        /// </summary>
        protected virtual void RemoveTouch(int fingerId)
        {
            touches.Remove(fingerId);

            if (touches.Count == 0)
            {
                EndTouch();
            }
        }
        /// <summary>
        /// 복수 터치 제거
        /// </summary>
        protected virtual void RemoveTouches(params int[] fingerIds)
        {
            for (int i = 0; i < fingerIds.Length; i++)
            {
                touches.Remove(fingerIds[i]);
            }

            if (touches.Count == 0)
            {
                EndTouch();
            }
        }

        /// <summary>
        /// 터치 종료 처리
        /// </summary>
        protected virtual void EndTouch()
        {
            ClearTouch();
        }
        
        /// <summary>
        /// 터치 클리어
        /// </summary>
        public override void ClearTouch()
        {
            Stop();

            touches.Clear();

            ClearMultiTouchData();
        }

        #region TouchData
        // 보유 터치 fingerId 리스트
        protected List<int> touches = new List<int>(10);
        // 터치 수
        public int Count => touches.Count;
        // 최대 터치 수
        [Range(1, 10)]
        [SerializeField] private int MaxTouchCount = 10;
        /// <summary>
        /// 인덱스 기반 터치 위치 반환
        /// </summary>
        protected Vector2 GetTouchPositionAt(int index)
        {
            if (index < 0 || index >= touches.Count)
                return Config.InvalidTouchPosition;
            else
                return GetTouchPosition(touches[index]);
        }
        /// <summary>
        /// 현재 보유한 터치 데이터 배열 반환
        /// </summary>
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

        #region Data
        // 멀티 터치 데이터
        protected MultiTouchData data = new MultiTouchData();

        /// <summary>
        /// 현재 멀티 터치 데이터 클리어
        /// </summary>
        protected virtual void ClearMultiTouchData()
        {
            data.Clear();
        }
        #endregion

        #region TouchRoutine
        // 터치 루틴 구동 중 여부
        private bool IsRunning => ieRoutine != null;

        /// <summary>
        /// 터치 코루틴 변수 및 함수
        /// </summary>
        private IEnumerator ieRoutine = null;
        protected abstract IEnumerator CoRoutine();

        /// <summary>
        /// 터치 구동
        /// </summary>
        protected virtual void Run()
        {
            Stop();

            ieRoutine = CoRoutine();
            StartCoroutine(ieRoutine);
        }
        /// <summary>
        /// 터치 구동 정지
        /// </summary>
        protected virtual void Stop()
        {
            if (ieRoutine != null)
            {
                StopCoroutine(ieRoutine);
                ieRoutine = null;
            }
        }
        #endregion
    }
}