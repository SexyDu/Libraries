using UnityEngine;

namespace SexyDu.Touch
{
    public abstract class TransformHandle : ITransformHandle
    {
        // 터치 바디
        protected ITransformHandler body
        {
            private set;
            get;
        }

        /// <summary>
        /// 터치 초기 설정
        /// </summary>
        public abstract void Setting();
        /// <summary>
        /// 터치 처리
        /// </summary>
        /// <returns>처리에 따른 위치 이동값(delta position)</returns>
        public abstract Vector2 Process();

        /// <summary>
        /// 바디 설정
        /// </summary>
        public virtual ITransformHandle SetBody(ITransformHandler body)
        {
            this.body = body;

            return this;
        }

        /// <summary>
        /// Unity Position Per One Pixel
        /// TouchCenter가 송출하는 화면의 1 픽셀 당 유니티 위치(크기) 값
        /// </summary>
        protected float UPPOP => TouchCenter.Config.UPPOP;
    }
}