using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Sample
{
    public class TestQuickSort : MonoBehaviour
    {
        [SerializeField] private int[] array;
        private ISorter<int> sorter = new QuickSorter<int>();

        [SerializeField] private string[] strings;
        private ISorter<string> stringSorter = new QuickSorter<string>();

        [SerializeField] private float[] floats;
        private ISorter<float> floatSorter = new QuickSorter<float>();

        private void OnEnable()
        {
            sorter.TrySort(array);
            stringSorter.TrySort(strings);
            floatSorter.TrySort(floats);
        }
    }
}