using UnityEngine;
using SexyDu.Tool;

namespace SexyDu.Sample
{
    public class TouchInputChecker : MonoBehaviour, IOnFrameTarget
    {
        private void OnEnable()
        {
            OnFrameContainer.Ins.Bind(this);
        }

        private void OnDisable()
        {
            OnFrameContainer.Ins.Unbind(this);
        }

        public void OnFrame()
        {
            Debug.LogFormat("on frame touch count : {0}", Input.touchCount);
        }
    }
}