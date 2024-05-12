using System;
using System.Collections;
using UnityEngine;
using SexyDu.Tool;

namespace SexyDu.Sample
{
    public class UseMonoHelperSample : MonoBehaviour
    {
        private IEnumerator CoSample()
        {
            while (true)
            {
                Debug.LogFormat("Running Sample Routine");

                yield return null;
            }
        }

        private IDisposable routine = null;

        private void StartSample()
        {
            StopSample();

            routine = MonoHelper.StartCoroutine(CoSample());
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