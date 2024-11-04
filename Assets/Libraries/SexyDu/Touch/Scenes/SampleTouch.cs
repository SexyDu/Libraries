using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Sample
{
    public class SampleTouch : MonoBehaviour
    {
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