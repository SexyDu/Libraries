using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 터치 전달자
    /// </summary>
    public class TouchMessanger : TouchTarget
    {
        // 전달 대상
        [SerializeField] private TouchTarget target;

        public override void ClearTouch()
        {
            target.ClearTouch();
        }

        public override void ReceiveTouch(int fingerId)
        {
            target.ReceiveTouch(fingerId);
        }
    }
}