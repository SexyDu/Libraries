using UnityEngine;

namespace SexyDu.Touch
{
    public struct InertialForceCollector : IClearable
    {
        private readonly InertialForce[] informations;
        private int index;

        public InertialForceCollector(int count)
        {
            informations = new InertialForce[count];
            index = 0;
        }

        public void Collect(Vector2 inertialForce)
        {
            Collect(inertialForce, Time.deltaTime);
        }

        public void Collect(Vector2 inertialForce, float deltaTime)
        {
            informations[index].Set(inertialForce, deltaTime);

            Next();
        }

        public InertialForce GetRecently()
        {
            int current = GetPrevious(index);
            
            while (current != index)
            {
                if (informations[current].Available)
                    return informations[current];
                else
                    current = GetPrevious(current);
            }

            informations[index].Clear();
            return informations[index];
        }

        public InertialForce GetHighest()
        {
            int highestIndex = 0;
            float highestValue = Mathf.Abs(informations[highestIndex].deltaPosition.x) + Mathf.Abs(informations[highestIndex].deltaPosition.y);

            for (int i = 1; i < informations.Length; i++)
            {
                float val = Mathf.Abs(informations[i].deltaPosition.x) + Mathf.Abs(informations[i].deltaPosition.y);
                if (highestValue < val)
                {
                    highestValue = val;
                    highestIndex = i;
                }
            }
            
            return informations[highestIndex];
        }

        public void Clear()
        {
            for (int i = 0; i < informations.Length; i++)
            {
                informations[i].Clear();
            }
        }

        private void Next()
        {
            index = GetNext(index);
        }

        private int GetNext(int index)
        {
            index++;
            if (index < informations.Length)
                return index;
            else
                return 0;
        }

        private int GetPrevious(int index)
        {
            index--;
            if (index < 0)
                return informations.Length - 1;
            else
                return index;
        }
    }
}