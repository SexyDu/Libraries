using UnityEngine;

namespace SexyDu.Touch
{
    public sealed class MultiTouchMover : MultiTouchActor
    {
        private Vector2 previous = Vector2.zero;
        private Vector3 position = Vector3.zero;

        public override Vector2 DeltaPositionAfterProcess => Vector2.zero;

        public override void Setting()
        {
            position = body.Target.position;

            previous = body.Data.center;
        }

        public override void Process()
        {
            Vector2 delta = Vector2.zero;

            delta += (body.Data.center - previous) * UPPOP;

            // Translate(delta);

            previous = body.Data.center;
        }
    }
}