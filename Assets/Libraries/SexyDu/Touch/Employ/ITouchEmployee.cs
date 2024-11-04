using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치의 추가 기능(더블클릭, 지속터치액션 등) 구현 시 사용될 인터페이스
    /// </summary>
    public interface ITouchEmployee
    {
        public void SetEmpoloyer(ITouchEmployer employer);

        public void Detect(int fingerId);

        public void Detect(int fingerId, Vector2 pos);

        public void Detect(int fingerId, Vector2 pos, float time);

        public void Disappear();

        public void Cancel();
    }
}