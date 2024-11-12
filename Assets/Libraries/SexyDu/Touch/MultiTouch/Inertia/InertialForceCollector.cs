using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 관성력 수집기
    /// </summary>
    public struct InertialForceCollector : IClearable
    {
        // 관성력 데이터 배열
        private readonly InertialForce[] informations;
        // 현재 관성 인덱스
        private int index;

        public InertialForceCollector(int count)
        {
            informations = new InertialForce[count];
            index = 0;
        }
        /// <summary>
        /// 관성 데이터 수집
        /// </summary>
        /// <param name="inertialForce">현재 이동 거리</param>
        public void Collect(Vector2 inertialForce)
        {
            Collect(inertialForce, Time.deltaTime);
        }
        /// <summary>
        /// 관성 데이터 수집
        /// </summary>
        /// <param name="inertialForce">현재 이동 거리</param>
        /// <param name="deltaTime">현재 이동 시간</param>
        public void Collect(Vector2 inertialForce, float deltaTime)
        {
            informations[index].Set(inertialForce, deltaTime);

            Next();
        }
        /// <summary>
        /// 최근의 관성 데이터 반환
        /// </summary>
        public InertialForce GetRecently()
        {
            int current = GetPrevious(index);

            while (current != index)
            {
                if (informations[current].Available)
                    return informations[current];
                else
                    current = GetPrevious(current);
            }

            informations[index].Clear();
            return informations[index];
        }
        /// <summary>
        /// 수집한 데이터 중 가장 큰 관성 데이터 반환
        /// </summary>
        public InertialForce GetHighest()
        {
            int highestIndex = 0;
            float highestValue = informations[highestIndex].Absolute;

            for (int i = 1; i < informations.Length; i++)
            {
                float val = informations[i].Absolute;
                if (highestValue < val)
                {
                    highestValue = val;
                    highestIndex = i;
                }
            }

            return informations[highestIndex];
        }
        /// <summary>
        /// 수집 데이터 클리어
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < informations.Length; i++)
            {
                informations[i].Clear();
            }
            index = 0;
        }
        /// <summary>
        /// 다음 인덱스로 변경
        /// </summary>
        private void Next()
        {
            index = GetNext(index);
        }
        /// <summary>
        /// 인자의 다음 인덱스 반환
        /// </summary>
        private int GetNext(int index)
        {
            index++;
            if (index < informations.Length)
                return index;
            else
                return 0;
        }
        /// <summary>
        /// 인자의 이전 인덱스 반환
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetPrevious(int index)
        {
            index--;
            if (index < 0)
                return informations.Length - 1;
            else
                return index;
        }
    }
}