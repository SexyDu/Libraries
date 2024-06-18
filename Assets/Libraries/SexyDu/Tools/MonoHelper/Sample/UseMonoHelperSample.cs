using System;
using System.Collections;
using UnityEngine;
using SexyDu.Tool;

namespace SexyDu.Sample
{
    public class UseMonoHelperSample : MonoBehaviour
    {
        [SerializeField] private int count;
        private IEnumerator CoSample(int number)
        {
            int count = 0;
            while (count < number)
            {
                Debug.LogFormat($"Running Sample Routine {count}");
                count++;
                yield return null;
            }
        }

        private IDisposable routine = null;

        private void StartSample()
        {
            StopSample();

            routine = MonoHelper.StartCoroutine(CoSample(count)).Subscribe(() => {
                Debug.LogFormat("코루틴 종료했다!");
            });
        }

        private void StopSample()
        {
            if (routine != null)
            {
                routine.Dispose();
                routine = null;
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Start"))
            {
                StartSample();
            }

            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Stop"))
            {
                StopSample();
            }
        }

        private void OnDestroy()
        {
            StopSample();
        }
    }
}