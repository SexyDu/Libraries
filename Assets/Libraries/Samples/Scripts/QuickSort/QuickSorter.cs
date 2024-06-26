#define USE_QUICKPART

using System;
using System.Collections.Generic;

namespace SexyDu.Sample
{
    public interface ISorter<T> where T : IComparable
    {
        public void TrySort(T[] array);
        public void Sort(T[] array);
    }
    
    public class QuickSorter<T> : ISorter<T> where T : IComparable
    {
        public void TrySort(T[] array)
        {
            if (array == null || array.Length.Equals(0))
                throw new NullReferenceException("array에 값이 없습니다.");
            else
                Sort(array);
        }

#if USE_QUICKPART
        public void Sort(T[] array)
        {
            Stack<QuickPart> stack = new Stack<QuickPart>();

            stack.Push(new QuickPart(0, array.Length - 1));

            do
            {
                QuickPart part = stack.Pop();

                if (part.Partitionable)
                {
                    int pivot = part.Partition(array);

                    stack.Push(new QuickPart(part.Low, pivot - 1));
                    stack.Push(new QuickPart(pivot + 1, part.High));
                }
            } while (stack.Count > 0);
        }
#else
        public void Sort(T[] array)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();

            stack.Push((0, array.Length - 1));

            do
            {
                var (low, high) = stack.Pop();

                if (low < high)
                {
                    int pivot = Partition(array, low, high);

                    stack.Push((low, pivot - 1));
                    stack.Push((pivot + 1, high));
                }
            } while (stack.Count > 0);
        }

        private int Partition(T[] array, int low, int high)
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

        private void Swap(T[] array, int a, int b)
        {
            T temporary = array[a];
            array[a] = array[b];
            array[b] = temporary;
        }
#endif
    }
}