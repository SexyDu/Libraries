using System.Collections;
using UnityEngine;

namespace SexyDu.Touch
{
    public class TouchKeepEmployee : TouchEmployee
    {
        // 터치 유지 시간
        private const float keepTime = 0.5f;

        public override void Detect(int fingerId, Vector2 pos, float time)
        {
            Run(new TouchTabInformation(fingerId, pos, time));
        }

        public override void Disappear()
        {
            Cancel();
        }

        public override void Cancel()
        {
            Stop();
        }

        #region Coroutine
        private IEnumerator ie = null;

        private IEnumerator co(TouchTabInformation tabInfo)
        {
            do
            {
                yield return null;

                if (!employer.ValidTouch(tabInfo.FingerId))
                    Cancel();
                    
            } while (Time.time - tabInfo.TouchTime < keepTime);

            Cancel();

            OnEvent();
        }

        private void Run(TouchTabInformation tabInfo)
        {
            Stop();

            ie = co(tabInfo);
            StartCoroutine(ie);
        }

        private void Stop()
        {
            if (ie is not null)
            {
                StopCoroutine(ie);
                ie = null;
            }
        }
        #endregion
    }
}