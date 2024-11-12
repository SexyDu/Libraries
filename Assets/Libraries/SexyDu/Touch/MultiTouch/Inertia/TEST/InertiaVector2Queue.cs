using UnityEngine;

namespace SexyDu.Touch
{
    public class InertiaVector2Queue
    {
        public InertiaVector2Queue(int capacity)
        {
            infos = new FrameInfo[capacity];
        }

        private FrameInfo[] infos;
        private int current = -1;

        public void Enqueue(Vector2 position)
        {
            Enqueue(position, Time.deltaTime);
        }

        public void Enqueue(Vector2 position, float time)
        {
            if (time > 0)
                infos[Next()].Set(position, time);
        }

        public int Next()
        {
            current++;

            if (current >= infos.Length)
                current = 0;

            return current;
        }

        public void ClearQueue()
        {
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i].Clear();
            }
        }

        public Vector2 Force()
        {
            Vector2 totalForce = Vector2.zero;
            float count = 0f;
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].Time > 0)
                {
                    totalForce += infos[i].Force;
                    count += 1f;
                }
            }

            return totalForce / count;
        }

        public Vector2 ForcePerOneSecond()
        {
            Vector2 totalForce = Vector2.zero;
            float totalTime = 0f;
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].Time > 0)
                {
                    totalForce += infos[i].Force;
                    totalTime += infos[i].Time;
                }
            }

            return totalForce * (1f / totalTime);
        }

        private struct FrameInfo
        {
            private Vector2 force;
            private float time;

            public Vector2 Force => force;
            public float Time => time;

            public void Set(Vector2 force, float time)
            {
                this.force = force;
                this.time = time;
            }

            public void Clear()
            {
                Set(Vector2.zero, 0f);
            }
        }
    }
}
