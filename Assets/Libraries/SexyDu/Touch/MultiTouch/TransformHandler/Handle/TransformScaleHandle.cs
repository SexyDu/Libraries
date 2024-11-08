using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티터치 크기 변경 수행 행들
    /// </summary>
    public class TransformScaleHandle : TransformHandle
    {
        // 이전 터치의 평균 거리값
        private float previous = 0f;
        // 대상 Transform의 크기
        private Vector3 scale = Vector3.zero;

        public override void Setting()
        {
            scale = body.Target.localScale;

            previous = GetDistanceAverage(body.Data);
        }

        public override Vector2 Process()
        {
            // 현재 터치의 평균 거리값
            float current = GetDistanceAverage(body.Data);
            // 이전 대비 변화된 거리값 (유니티 크기 기준으로 변환)
            float delta = (current - previous) * UPPOP;

            // 변경 전 크기값 (이 후 delta position 계산에 사용)
            float scaleBefore = scale.x;

            // 크기 변경
            Change(delta);
            // 이전 크기값 변경
            previous = current;

            // 변경 후 크기값
            float scaleAfter = scale.x;
            // 변경 전후 비율
            float ratio = scaleAfter / scaleBefore;
            // 현재 터치 센터의 유니티 위치값
            Vector2 centerUnit = TouchCenter.Config.ConvertUnityPosition(body.Data.center);
            // 크기 변경 전의 터치 센터와 대상간의 위치 차이
            Vector2 differenceBefore = new Vector2(
                body.Target.position.x - centerUnit.x,
                body.Target.position.y - centerUnit.y
            );
            // 크기 변경 후 이를 고려한 터치센터와 대상간의 위치 아치
            Vector2 differenceAfter = differenceBefore * ratio;
            // 위 차이를 빼서 deltaPosition 계산 후 반환
            return differenceAfter - differenceBefore;
        }

        /// <summary>
        /// 멀티터치의 센터 기준 평균 거리값
        /// </summary>
        private float GetDistanceAverage(MultiTouchData data)
        {
            float sum = 0f;
            for (int i = 0; i < data.Count; i++)
            {
#if false
                float a = Mathf.Pow(data.center.x - data.Touches[i].position.x, 2);
                float b = Mathf.Pow(data.center.y - data.Touches[i].position.y, 2);
                float distance = Mathf.Sqrt(a + b);
                sum += distance;
#else
                sum += Vector2.Distance(data.center, data.Touches[i].position);
#endif
            }

            return sum *= data.CountForMultiple;
        }
        /// <summary>
        /// 대상 크기 변경
        /// </summary>
        private void Change(float delta)
        {
            Set(scale.x + delta);
        }
        /// <summary>
        /// 대상 크기 설정
        /// </summary>
        /// <param name="size"></param>
        private void Set(float size)
        {
            scale.x = scale.y = size;
            body.Target.localScale = scale;
        }
    }
}