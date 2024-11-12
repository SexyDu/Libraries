using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 이동 관성력 적용 트랜스폼 핸들러
    /// </summary>
    public class TransformInertiaHandler : TransformHandler, IInertiaTarget
    {
        [Header("Inertia")]
        [SerializeField] private InertiaProcessor inertiaProcessor; // 관성 작동기
        private IInertiaProcessor Processor => inertiaProcessor; // 관성 작동기 인터페이스

        // 관성력 수집기
        private InertialForceCollector inertialForceCollector = new InertialForceCollector(2);

        /// <summary>
        /// 초기 설정
        /// : TransformHandler
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Processor.Set((IInertiaTarget)this).Set((MonoBehaviour)this);
        }

        /// <summary>
        /// 터치된 fingerId 수신
        /// : MultiTouchBase
        /// </summary>
        public override void ReceiveTouch(int fingerId)
        {
            Processor.Stop();

            base.ReceiveTouch(fingerId);
        }

        /// <summary>
        /// 터치 종료 처리
        /// : MultiTouchBase
        /// </summary>
        protected override void EndTouch()
        {
            // EndTouch를 수행하기 전 관성력 가져오기
            /// base.EndTouch, ClearTouch에서 inertialForceCollector.Clear를 수행하기 때문에 미리 가져온다
            // InertialForce recently = inertialForceCollector.GetRecently();
            InertialForce recently = inertialForceCollector.GetHighest();

            base.EndTouch();

            Processor.Run(recently.deltaPosition, recently.deltaTime);
        }

        /// <summary>
        /// 터치 클리어
        /// : MultiTouchBase
        /// </summary>
        public override void ClearTouch()
        {
            base.ClearTouch();

            Processor.Stop();
            inertialForceCollector.Clear();
        }
        /// <summary>
        /// 대상 위치 이동
        /// : AbstractTransformHandler
        /// </summary>
        protected override void Translate(Vector2 delta)
        {
            base.Translate(delta);

            inertialForceCollector.Collect(delta);
        }

        /// <summary>
        /// 관성 적용
        /// </summary>
        /// <param name="force">관성력</param>
        public void Inertia(Vector2 force)
        {
            // 여기서는 관성에 따른 위치이동만 하고 Collect 시키지 않기 위해 base.Translate 호출
            base.Translate(force);
        }
    }
}