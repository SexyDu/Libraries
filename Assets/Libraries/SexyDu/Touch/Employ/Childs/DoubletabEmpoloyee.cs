using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치 더블탭 추가 기능 클래스
    /// </summary>
    public class DoubletabEmpoloyee : TouchEmployee
    {
        // 더블탭간 유효 시간
        private const float gapTime = 0.3f;

        // 첫번째 탭 시작 정보
        private TouchTabInformation tabInfo = null;
        private bool doubled = false;

        public override void Detect(int fingerId, Vector2 pos, float time)
        {
            if (Check(time))
            {
                doubled = true;
            }
            else
            {
                tabInfo = new TouchTabInformation(fingerId, pos, time);
                doubled = false;
            }
        }

        public override void Disappear()
        {
            if (Check(Time.time))
            {
                if (doubled)
                    OnEvent();
            }
            else
            {
                Cancel();
            }
        }

        public override void Cancel()
        {
            tabInfo = null;

            doubled = false;
        }

        /// <summary>
        /// 터치 시간 유효 체크
        /// </summary>
        private bool Check(float time)
        {
            /// tabInfo가 없는 경우
            if (tabInfo is null)
                return false;
            else
                // 전달받은 시간과 터치 시간이 gapTime보다 작은 경유 유효
                return (time - tabInfo.TouchTime) < gapTime;
        }
    }
}