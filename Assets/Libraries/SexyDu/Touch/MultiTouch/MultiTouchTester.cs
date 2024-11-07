using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public class MultiTouchTester : TouchTarget, ITransformHandler
    {
        public override void ReceiveTouch(int fingerId)
        {
            if (touches.Count < MaxTouchCount)
            {
                touches.Add(fingerId);

                if (!IsRunning)
                    RunRoutine();
            }
        }
        /// <summary>
        /// 터치 제거
        /// </summary>
        /// <param name="fingerId"></param>
        protected virtual void RemoveTouch(int fingerId)
        {
            touches.Remove(fingerId);
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
                StopRoutine();

                data.Clear();
                previous = Vector2.zero;
            }
        }
        /// <summary>
        /// 터치 클리어
        /// </summary>
        public override void ClearTouch()
        {
            touches.Clear();
        }

        #region Transform
        // 터치 transform 대상
        [SerializeField] private Transform target;
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

        #region TouchData
        // 보유 터치 fingerId 리스트
        protected List<int> touches = new List<int>(10);
        // 터치 수
        public int Count => touches.Count;
        // 최대 터치 수
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

        #region TouchRoutine
        // 이전 위치값
        private Vector2 previous = Vector2.zero;
        // 멀티 터치 데이터
        private MultiTouchData data = new MultiTouchData();
        /// <summary>
        /// 멀티 터치 데이터 Property
        /// : IMultiTouchBody
        /// </summary>
        public MultiTouchData Data => data;
        // 터치 루틴 구동 중 여부
        private bool IsRunning => ieRoutine != null;

        // [TEST] 스케일액터
        private TransformScaleHandle scaleActor = new TransformScaleHandle();
        //
        private TransformAngleHandle angleActor = new TransformAngleHandle();

        /// <summary>
        /// 터치 루틴 및 코루틴
        /// </summary>
        private IEnumerator ieRoutine = null;
        private IEnumerator CoRoutine()
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
                previous = data.center;
                scaleActor.SetBody(this);
                scaleActor.Setting();

                angleActor.SetBody(this);
                angleActor.Setting();
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
                Vector2 delta = Vector2.zero;

                scaleActor.Process();
                delta += scaleActor.DeltaPositionAfterProcess;

                angleActor.Process();
                delta += angleActor.DeltaPositionAfterProcess;

                delta += (data.center - previous) * UPPOP;

                Translate(delta);

                previous = data.center;
            }
        }
        /// <summary>
        /// 터치 구동
        /// </summary>
        private void RunRoutine()
        {
            StopRoutine();

            ieRoutine = CoRoutine();
            StartCoroutine(ieRoutine);
        }
        /// <summary>
        /// 터치 구동 정지
        /// </summary>
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
}