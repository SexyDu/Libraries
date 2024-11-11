using UnityEngine;

namespace SexyDu.Touch
{
    /// <summary>
    /// 기본 트랜스폼 핸들러
    /// * 위치 변경, 크기 조절
    /// </summary>
    public class TransformHandler : AbstractTransformHandler, ITransformHandler, IReleasable
    {
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
            if (x < minimumPosition.x)
                position.x = minimumPosition.x;
            else if (x > maximumPosition.x)
                position.x = maximumPosition.x;
            else
                position.x = x;

            // y축 제한 위치 보정
            if (y < minimumPosition.y)
                position.y = minimumPosition.y;
            else if (y > maximumPosition.y)
                position.y = maximumPosition.y;
            else
                position.y = y;

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
        // 최소 위치값
        [SerializeField] private Vector2 minimumPosition;
        // 최대 위치값
        [SerializeField] private Vector2 maximumPosition;

        /// <summary>
        /// 제한 위치값 설정
        /// </summary>
        public TransformHandler SetLimitedPosition(Vector2 minimum, Vector2 maximum)
        {
            minimumPosition = minimum;
            maximumPosition = maximum;

            return this;
        }
        /// <summary>
        /// 최소 위치값 설정
        /// </summary>
        public TransformHandler SetMinimumPosition(Vector2 val)
        {
            minimumPosition = val;

            return this;
        }
        /// <summary>
        /// 최대 위치값 설정
        /// </summary>
        public TransformHandler SetMaximumPosition(Vector2 val)
        {
            maximumPosition = val;

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
        /// 제한 크기값 설정
        /// </summary>
        public TransformHandler SetLimitedScale(float minimum, float maximum)
        {
            minimumScale = minimum;
            maximumScale = maximum;

            return this;
        }
        /// <summary>
        /// 최소 크기값 설정
        /// </summary>
        public TransformHandler SetMinimumScale(float val)
        {
            minimumScale = val;

            return this;
        }
        /// <summary>
        /// 최대 크기값 설정
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public TransformHandler SetMaximumScale(float val)
        {
            maximumScale = val;

            return this;
        }
        #endregion

#if false
        #region Handles
        [Header("Handles")]
        [SerializeField] private HandleType[] handleTypes; // 사용할 핸들 타입

        /// <summary>
        /// 핸들 초기 설정
        /// </summary>
        private void InitializeHandles()
        {
            List<ITransformHandle> list = new List<ITransformHandle>();

            for (int i = 0; i < handleTypes.Length; i++)
            {
                try
                {
                    list.Add(CreateHandle(handleTypes[i]).SetBody(this));
                }
                catch (NotSupportedException e)
                {
                    Debug.LogFormat("[Transform Handle 추가 실패] {0}", e.Message);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            RegisterHandles(list.ToArray());
        }

        /// <summary>
        /// 핸들 타입에 맞는 핸들 생성 및 인터페이스 반환
        /// </summary>
        /// <param name="type">핸들 타입</param>
        /// <returns>생성된 핸들 인터페이스</returns>
        /// <exception cref="NotSupportedException">타입이 없는 경우의 Exception</exception>
        private ITransformHandle CreateHandle(HandleType type)
        {
            switch (type)
            {
                case HandleType.Position:
                    return new TransformPositionHandle();
                case HandleType.Scale:
                    return new TransformScaleHandle();
                case HandleType.Angle:
                    return new TransformAngleHandle();
                default:
                    throw new NotSupportedException($"해당 HandleType({type})에 맞는 클래스가 없습니다.");
            }
        }
        #endregion


        [Serializable]
        private enum HandleType : byte
        {
            Unknown = 0,
            Position = 1, // 위치 이동
            Scale = 2, // 크기 변경
            Angle = 3, // 각도 조절
        }
#endif

    }
}