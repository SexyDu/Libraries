using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 한개의 터치 데이터
    /// </summary>
    public struct TouchData
    {
        // finger id
        public readonly int fingerId;
        // 터치 위치
        public readonly Vector2 position;
        // 유효 터치 여부
        public bool IsValid => ITouchCenter.Config.ValidateTouchPosition(position);

        public TouchData(int fingerId)
        {
            this.fingerId = fingerId;
            position = ITouchCenter.Config.GetTouchPosition(this.fingerId);
        }

        public TouchData(int fingerId, Vector2 position)
        {
            this.fingerId = fingerId;
            this.position = position;
        }
    }
}