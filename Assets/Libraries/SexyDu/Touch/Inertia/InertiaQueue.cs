namespace SexyDu.Touch
{
    public class InertiaQueue
    {
        public InertiaQueue(int capacity)
        {
            infos = new FrameInfo[capacity];
        }

        private FrameInfo[] infos;
        private int current = -1;

        public void Enqueue(float position)
        {
            Enqueue(position, UnityEngine.Time.deltaTime);
        }

        public void Enqueue(float position, float time)
        {
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

        public float PositionPerOneSecond()
        {
            float totalPosition = 0f;
            float totalTime = 0f;
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].Time > 0)
                {
                    totalPosition += infos[i].Position;
                    totalTime += infos[i].Time;
                }
            }

            return totalPosition * (1f / totalTime);
        }

        private struct FrameInfo
        {
            private float position;
            private float time;

            public float Position => position;
            public float Time => time;

            public void Set(float position, float time)
            {
                this.position = position;
                this.time = time;
            }

            public void Clear()
            {
                Set(0f, 0f);
            }
        }
    }
}
