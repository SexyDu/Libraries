using SexyDu.Touch;
using UnityEngine;

namespace SexyDu.Sample
{
    public class SampleTouch : MonoBehaviour
    {
        [SerializeField] private TransformHandler[] touchHandlers;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            Vector2 unitMaximumArea = ITouchCenter.Config.GetUnitArea();
            Vector2 unitMinimumArea = -unitMaximumArea;
            for (int i = 0; i < touchHandlers.Length; i++)
            {
                touchHandlers[i].SetMinimumPosition(unitMinimumArea).SetMaximumPosition(unitMaximumArea);
            }
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