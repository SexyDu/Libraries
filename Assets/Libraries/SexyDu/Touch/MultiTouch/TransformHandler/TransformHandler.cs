using SexyDu.Tool;
using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 기본 트랜스폼 핸들러
    /// * 위치 변경, 크기 조절
    /// </summary>
    public class TransformHandler : AbstractTransformHandler, ITransformHandler, IReleasable
    {
        [Header("TransformHandler")]
        [SerializeField] private bool onAwakeInit = true;

        protected virtual void Awake()
        {
            if (onAwakeInit)
                Initialize();
        }

        protected virtual void OnDestroy()
        {
            Release();
        }

        /// <summary>
        /// 초기 설정
        /// </summary>
        public virtual void Initialize()
        {
            InitializeHandles();
        }

        /// <summary>
        /// 리소스 해제
        /// </summary>
        public virtual void Release()
        {
            positionHandle = null;
            scaleHandle = null;
        }

        #region Transform
        /// <summary>
        /// 대상 위치 설정
        /// : AbstractTransformHandler
        /// </summary>
        protected override void SetPosition(float x, float y)
        {
            // x축 제한 위치 보정
            position.x = positionLimiter.CorrectX(x);

            // y축 제한 위치 보정
            position.y = positionLimiter.CorrectY(y);

            target.position = position;
        }
        #endregion

        #region Handles
        /// <summary>
        /// 핸들 초기 설정
        /// </summary>
        protected virtual void InitializeHandles()
        {
            positionHandle = new TransformPositionHandle();
            positionHandle.SetBody(this);

            scaleHandle = new TransformLimitedScaleHandle(minimumScale, maximumScale);
            scaleHandle.SetBody(this);
        }

        /// <summary>
        /// 전체 핸들 설정
        /// : AbstractTransformHandler
        /// </summary>
        protected override void SettingHandles()
        {
            positionHandle.Setting();
            scaleHandle.Setting();
        }
        /// <summary>
        /// 전체 핸들 업무 수행
        /// : AbstractTransformHandler
        /// </summary>
        /// <returns>업무 수행에 따른 위치 이동값</returns>
        protected override Vector2 ProcessHandles()
        {
            Vector2 deltaPosition = positionHandle.Process();
            deltaPosition += scaleHandle.Process();

            return deltaPosition;
        }
        #endregion

        #region Handle - Position
        // 위치 조절 핸들
        private TransformPositionHandle positionHandle;

        [Header("Position")]
        [SerializeField] private LimiterVector2 positionLimiter;

        /// <summary>
        /// 최소 위치값 설정
        /// </summary>
        public TransformHandler SetMinimumPosition(Vector2 val)
        {
            positionLimiter.SetMinimum(val);

            return this;
        }
        /// <summary>
        /// 최대 위치값 설정
        /// </summary>
        public TransformHandler SetMaximumPosition(Vector2 val)
        {
            positionLimiter.SetMaximum(val);

            return this;
        }
        #endregion

        #region Handle - Scale
        // 크기 조절 핸들
        private TransformLimitedScaleHandle scaleHandle;

        [Header("Scale")]
        [SerializeField] private float minimumScale;
        [SerializeField] private float maximumScale;
        
        /// <summary>
        /// 최소 크기값 설정
        /// </summary>
        public TransformHandler SetMinimumScale(float val)
        {
            minimumScale = val;
            scaleHandle.SetMinimum(minimumScale);

            return this;
        }
        /// <summary>
        /// 최대 크기값 설정
        /// </summary>
        public TransformHandler SetMaximumScale(float val)
        {
            maximumScale = val;
            scaleHandle.SetMaximum(maximumScale);

            return this;
        }
        #endregion
    }
}