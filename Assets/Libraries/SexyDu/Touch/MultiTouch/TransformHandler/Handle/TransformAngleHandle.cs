using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 멀티터치 각도 변경 수행 핸들
    /// </summary>
    public sealed class TransformAngleHandle : TransformHandle
    {
        private Vector3 eulerAngles = Vector3.zero;

        public override void Setting()
        {
            eulerAngles = body.Target.localEulerAngles;

            previous = GetAngles(body.Data.center, body.Data.Touches);
        }

        public override Vector2 Process()
        {
            // 현재 터치의 각도 배열
            float[] current = GetAngles(body.Data.center, body.Data.Touches);

            // 회전된 평균 각도값 계산
            float averageAngle = 0f;
            for (int i = 0; i < current.Length; i++)
            {
                averageAngle += NormalizeAngle(current[i] - previous[i]);
            }
            averageAngle *= body.Data.CountForMultiple;

            // 회전된 평균 각도값을 이용한 대상 각도 변경
            Change(averageAngle);

            // 현재 각도 배열을 이전으로 대치
            previous = current;

            // 각도가 변경된 경우 각도 변경에 따른 위치 변경값 설정
            if (averageAngle != 0f)
            {
                // 현재 오브젝트 위치값
                Vector2 currentPoint = body.Target.position;
                // 현재 터치 센터의 유니티 위치값
                Vector2 centerUnit = TouchCenter.Config.ConvertUnityPosition(body.Data.center);
                // 새로 계산된 오브젝트 위치값
                Vector2 newPoint = RotatePoint(currentPoint, centerUnit, averageAngle);

                // 새 위치값에서 현재 위치값을 빼서 변화량 계산
                return newPoint - currentPoint;
            }
            else
                return Vector2.zero;
        }
        /// <summary>
        /// 대상 각도 변경
        /// </summary>
        private void Change(float delta)
        {
            Set(eulerAngles.z + delta);
        }
        /// <summary>
        /// 대상 각도 설정
        /// </summary>
        private void Set(float angle)
        {
            eulerAngles.z = angle;
            body.Target.localEulerAngles = eulerAngles;
        }

        // 각도 보정에 들어갈 PI값
        private const float PI = 3.14159265f;
        // 하위에서 계산에 사용될 PI 나눗셈값
        private const float PI_FORMULT = 1f / PI; // 0.31830989f
        // Atan2로 계산된 값을 -180 ~ 180값으로 변경하기 위한 파라미터 값
        private const float AngleParameter = 180f * PI_FORMULT;
        // 이전 터치의 각도 배열
        private float[] previous = null;

        /// <summary>
        /// 두 점에 대한 각도값 반환
        /// </summary>
        /// <param name="center">중심점</param>
        /// <param name="subject">대상점</param>
        /// <returns>각도값</returns>
        private float GetAngle(Vector2 center, Vector2 subject)
        {
            return Mathf.Atan2(subject.y - center.y, subject.x - center.x) * AngleParameter;
        }
        /// <summary>
        /// 터치에 대한 각도 배열 반환
        /// </summary>
        /// <param name="center">중심점</param>
        /// <param name="subjects">터치 데이터 배열</param>
        /// <returns>각도 배열</returns>
        private float[] GetAngles(Vector2 center, TouchData[] subjects)
        {
            float[] angles = new float[subjects.Length];

            for (int i = 0; i < angles.Length; i++)
            {
                angles[i] = GetAngle(center, subjects[i].position);
            }

            return angles;
        }

        /// <summary>
        /// 계산을 위한 각도값 보정
        /// * 어떠한 수치가 들어와도 계산 각도에 맞도록 -180 ~ 180의 수치로 변환
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public float NormalizeAngle(float angle)
        {
            if (angle < -180f || angle > 180f)
            {
                // 360으로 나눈 나머지를 구합니다.
                angle %= 360;

                // 각도가 180보다 크면 -180~180 범위로 조정합니다.
                if (angle > 180)
                    angle -= 360;
                else if (angle < -180)
                    angle += 360;
            }

            return angle;
        }

        /// <summary>
        /// 두 점의 각도 변경에 따른 변경 위치값 반환
        /// </summary>
        /// <param name="point">대상점</param>
        /// <param name="center">중심점</param>
        /// <param name="angle">변경된 각도</param>
        public Vector2 RotatePoint(Vector2 point, Vector2 center, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;

            // 상대 좌표
            Vector2 relativePoint = point - center;

            // 회전 적용
            float rotatedX = relativePoint.x * Mathf.Cos(rad) - relativePoint.y * Mathf.Sin(rad);
            float rotatedY = relativePoint.x * Mathf.Sin(rad) + relativePoint.y * Mathf.Cos(rad);

            // 최종 좌표
            Vector2 newPoint = new Vector2(rotatedX, rotatedY) + center;
            return newPoint;
        }
    }
}