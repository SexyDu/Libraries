using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace SexyDu.Sample
{
    public class SampleTouch : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public void OnClick()
        {
            Debug.Log("클릭");
        }

        public void OnDoubleTab()
        {
            Debug.Log("더블클릭");
        }

        public void OnKeepPress()
        {
            Debug.Log("터치유지");
        }
    }
}