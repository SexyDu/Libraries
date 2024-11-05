using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class MultiTouchActor : IMultiTouchActor
    {
        // 터치 바디
        public IMultiTouchBody body
        {
            set;
            protected get;
        }

        /// <summary>
        /// Process처리로 인해 발생한 위치 이동값
        /// </summary>
        public abstract Vector2 DeltaPositionAfterProcess { get; }

        /// <summary>
        /// 터치 초기 설정
        /// </summary>
        public abstract void Setting();
        /// <summary>
        /// 터치 처리
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// 바디 설정
        /// </summary>
        public virtual void SetBody(IMultiTouchBody body)
        {
            this.body = body;
        }

        /// <summary>
        /// Unity Position Per One Pixel
        /// TouchCenter가 송출하는 화면의 1 픽셀 당 유니티 위치(크기) 값
        /// </summary>
        protected float UPPOP => TouchCenter.Config.UPPOP;
    }
}