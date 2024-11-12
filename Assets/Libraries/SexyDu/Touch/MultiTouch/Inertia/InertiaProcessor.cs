using System;
using System.Collections;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 관성력 작동기
    /// </summary>
    [Serializable]
    public class InertiaProcessor : IInertiaProcessor
    {
        /// <summary>
        /// 계산을 위한 프레임 정보
        /// * 아래 나와 있는 바와 같이 BaseFrameRate의 프레임레이트를 가진다고 가정하여 계산하기 위함 (기준값임)
        /// </summary>
        private const int BaseFrameRate = 60; // 기준 프레임 레이트
        private const float BaseDeltaTime = 1f / (float)BaseFrameRate; // 기준 프레임 당 시간(초)


        [Range(0f, 1f)]
        [SerializeField] private float decelerationRate; // 기준 프레임 당 감속율
        [SerializeField] private float inertiaBreak; // 최소 감속 수치

        // 관성 대상
        private IInertiaTarget target = null;
        // 코루틴 워커
        private MonoBehaviour worker = null;

        /// <summary>
        /// 관성 대상 설정
        /// : IInertiaProcessor
        /// </summary>
        public IInertiaProcessor Set(IInertiaTarget target)
        {
            this.target = target;

            return this;
        }
        /// <summary>
        /// 코루틴 워커 설정
        /// : IInertiaProcessor
        /// </summary>
        public IInertiaProcessor Set(MonoBehaviour worker)
        {
            this.worker = worker;

            return this;
        }

        /// <summary>
        /// 감속 수치에 따라 관성력을 유지할지에 대한 여부 반환
        /// </summary>
        private bool IsContinue(Vector2 inertiaForce)
        {
            inertiaForce.x = Mathf.Abs(inertiaForce.x);
            inertiaForce.y = Mathf.Abs(inertiaForce.y);

            return inertiaForce.x > inertiaBreak || inertiaForce.y > inertiaBreak;
        }

        /// <summary>
        /// 관성 동작 코루틴 변수 및 함수
        /// </summary>
        private IEnumerator ieInertia = null;
        private IEnumerator CoInertia(Vector2 force)
        {
            float reverseRate = 1f - decelerationRate;
            do
            {
                yield return null;

                // 한 프레임에 대한 감속 갭 계산
                /// 갭 계산 후 해당 갭을 기준 프레임에 맞춰 보정 (GetDeltaPositionOnFrame)
                Vector2 variance = GetDeltaPositionOnFrame(force * reverseRate, Time.deltaTime);
                // 감속
                force -= variance;
                // 관성 이동
                target.Inertia(force);
            } while (IsContinue(force));
        }

        /// <summary>
        /// 관성 동작 실행
        /// : IInertiaProcessor
        /// </summary>
        /// <param name="inertiaForce">관성력</param>
        /// <param name="deltaTime">관성력 작용 시간(deltaTime)</param>
        public void Run(Vector2 inertialForce, float deltaTime)
        {
            Stop();
            
            if (inertialForce != Vector2.zero && deltaTime > 0f)
            {
                ieInertia = CoInertia(GetDeltaPositionOnFrame(inertialForce, deltaTime));
                worker.StartCoroutine(ieInertia);
            }
        }

        /// <summary>
        /// 관성 동작 종료
        /// : IInertiaProcessor
        /// </summary>
        public void Stop()
        {
            if (ieInertia != null)
            {
                worker.StopCoroutine(ieInertia);
                ieInertia = null;
            }
        }
        /// <summary>
        /// 실제 프레임 시간을 고려하여 관성력 보정
        /// </summary>
        /// <param name="inertialForce">관성력</param>
        /// <param name="deltaTime">관성력 작용 시간(deltaTime)</param>
        private Vector2 GetDeltaPositionOnFrame(Vector2 inertialForce, float deltaTime)
        {
            /// 실제 관성력 : 관성력 작용 시간 = 기준 관성력 : 기준 관성력 작용 시간(기준 프레임 당 시간)
            /// inertialForce : deltaTime = [Result] : BaseDeltaTime
            /// [Result] * deltaTime = inertiaForce * BaseDeltaTime
            /// [Result] = (inertiaForce * BaseDeltaTime) / deltaTime
            return inertialForce * BaseDeltaTime / deltaTime;
        }
    }
}