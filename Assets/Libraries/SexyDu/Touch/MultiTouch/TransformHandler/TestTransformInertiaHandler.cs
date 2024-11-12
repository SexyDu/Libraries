using UnityEngine;

namespace SexyDu.Touch
{
    public class TestTransformInertiaHandler : TransformHandler, IInertiaTarget
    {
        [Header("Inertia")]
        [SerializeField] private TestInertiaProcessor inertiaProcessor;
        private InertialForceCollector inertialForceCollector = new InertialForceCollector(2);

        public override void Initialize()
        {
            base.Initialize();

            inertiaProcessor.Set((IInertiaTarget)this).Set((MonoBehaviour)this);
        }

        public override void ReceiveTouch(int fingerId)
        {
            inertiaProcessor.Stop();

            base.ReceiveTouch(fingerId);

            ClearTestText();
        }

        protected override void EndTouch()
        {
            // EndTouch를 수행하기 전 관성력 가져오기
            /// base.EndTouch, ClearTouch에서 inertialForceCollector.Clear를 수행하기 때문에 미리 가져온다
            // InertialForce recently = inertialForceCollector.GetRecently();
            InertialForce recently = inertialForceCollector.GetHighest();

            base.EndTouch();

            inertiaProcessor.Run(recently.deltaPosition, recently.deltaTime);

            SetTestText(recently);
        }

        public override void ClearTouch()
        {
            base.ClearTouch();

            inertiaProcessor.Stop();
            inertialForceCollector.Clear();
        }

        protected override void Translate(Vector2 delta)
        {
            base.Translate(delta);

            inertialForceCollector.Collect(delta);
        }

        public void Inertia(Vector2 force)
        {
            // 여기서 Collect 시키지 않기 위해 base.Translate 호출
            base.Translate(force);
        }

        #region TEST
        [SerializeField] TMPro.TMP_Text textMesh;

        private void SetTestText(InertialForce force)
        {
            textMesh.SetText($"x: {force.deltaPosition.x}\ny: {force.deltaPosition.y}\ndt: {force.deltaTime}");
        }
        
        private void ClearTestText()
        {
            textMesh.text = string.Empty;
        }
        #endregion
    }
}