using System;

namespace SexyDu.Sample
{
    public struct QuickPart
    {
        private readonly int low;
        private readonly int high;

        public int Low => low;
        public int High => high;
        public bool Partitionable => low < high;

        public QuickPart(int low, int high)
        {
            this.low = low;
            this.high = high;
        }

        public int Partition<T>(T[] array) where T : IComparable
        {
            T pivot = array[high];
            int left = low - 1;

            for (int i = low; i < high; i++)
            {
                if (array[i].CompareTo(pivot) <= 0)
                {
                    left++;
                    Swap(array, left, i);
                }
            }

            int pivotIndex = left + 1;
            Swap(array, pivotIndex, high);

            return pivotIndex;
        }

        private void Swap<T>(T[] array, int a, int b)
        {
            T temporary = array[a];
            array[a] = array[b];
            array[b] = temporary;
        }
    }
}