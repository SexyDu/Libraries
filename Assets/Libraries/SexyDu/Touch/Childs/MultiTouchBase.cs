using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티 터치 기반 클래스
    /// </summary>
    public /*abstract*/ class MultiTouchBase : TouchTarget, IMultiTouchBody
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
        private MultiTouchScaleActor scaleActor = new MultiTouchScaleActor();

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

                .
                // 이제 각도 변경 하자!!

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

    /// <summary>
    /// 다중 터치 데이터
    /// </summary>
    public struct MultiTouchData
    {
        // 터치 배열
        private TouchData[] touches;
        // 터치 수
        private int count;
        // 나눗셈을 곱셉으로 계산하기 위한 터치수 값
        private float countForMult;

        // 센터 위치값
        public Vector2 center
        {
            private set;
            get;
        }

        // [Property] 터치 배열
        public TouchData[] Touches => touches;

        // [Property] 터치 수
        public int Count => count;
        // [Property] 나눗셈을 곱셉으로 계산하기 위한 터치수 값 프로퍼티
        public float CountForMultiple => countForMult;

        // 터치 유효 여부
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

        /// <summary>
        /// 터치 수 설정
        /// </summary>
        public void Set(int count)
        {
            this.count = count;
            countForMult = 1f / (float)count;
        }
        /// <summary>
        /// 터치 배열 설정
        /// </summary>
        /// <param name="touches"></param>
        public void Set(TouchData[] touches)
        {
            // 터치 설정
            this.touches = touches;

            // 터치 센터 설정
            Vector2 sum = Vector2.zero;
            for (int i = 0; i < this.touches.Length; i++)
            {
                sum += touches[i].position;
            }
            center = sum * countForMult;
        }

        // 터치 클리어
        public void Clear()
        {
            count = 0;
            countForMult = 0f;
            touches = null;
        }

        /// <summary>
        /// 유효하지 않은 터치의 fingerId 배열 반환
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// 한개의 터치 데이터
    /// </summary>
    public struct TouchData
    {
        // finger id
        public readonly int fingerId;
        // 터치 위치
        public readonly Vector2 position;
        // 유효 터치 여부
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