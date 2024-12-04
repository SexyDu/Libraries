#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public class TouchConfig
    { 
        // 마우스 좌클릭 터치 ID
        public const int MouseIdLeft = 100;
        // 마우스 우클릭 터치 ID
        public const int MouseIdRight = 101;
        
        public TouchConfig()
        {
            EventSystem = new TouchEventSystem();
        }

        #region TouchCenter Management
        // 활성화된 전체 터치 센터
        private List<TouchCenter> touchCenters = new List<TouchCenter>();
        // 프로젝트 기본 OrthographicSize
        private const float DefaultOrthographicSize = 5f;

        private float OrthographicSize
        {
            get
            {
                if (MainTouchCenter == null)
                    return DefaultOrthographicSize;
                else
                    return MainTouchCenter.MainCam.orthographicSize;
            }
        }

        // 메인 터치 센터
        public TouchCenter MainTouchCenter
        {
            get
            {
                if (touchCenters.Count > 0)
                    return touchCenters[touchCenters.Count - 1];
                else
                    return null;
            }
        }

        /// <summary>
        /// 터치 센터 추가 함수
        /// </summary>
        /// <param name="touchCenter">터치 센터</param>
        public void AddTouchCenter(TouchCenter touchCenter)
        {
            // 리스트에 추가
            touchCenters.Add(touchCenter);
            // 메인 터치 센터 정보 설정
            SetMainTouchCenterInfo();
        }/// <summary>
         /// 터치 센터 삭제 함수
         /// </summary>
         /// <param name="touchCenter">터치 센터</param>
        public void RemoveTouchCenter(TouchCenter touchCenter)
        {
            // 삭제될 터치센터가 현재 메인 터치센터인지 확인
            bool isMain = touchCenter.Equals(MainTouchCenter);
            // 터치 센터에서 삭제
            touchCenters.Remove(touchCenter);
            // 메인 터치 센터 정보 설정
            /// 삭제된게 메인인 경우 메인 터치 센터가 변경된 것이기 때문에 해당 변수 전달
            if (isMain)
                SetMainTouchCenterInfo();
        }
        /// <summary>
        /// 메인 터치 센터 정보를 설정하는 함수
        /// </summary>
        private void SetMainTouchCenterInfo()
        {
            SettingScreenInformation();

            for (int i = 0; i < touchCenters.Count - 1; i++)
            {
                touchCenters[i].Stop();
            }

            MainTouchCenter?.Run();
        }
        #endregion



        /// <summary>
        /// 1픽셀당 유니티 오브젝트 크기/위치값
        /// </summary>
        private float unityPositionPerOnePixel;
        public float UPPOP
        {
            get { return this.unityPositionPerOnePixel; }
        }

        /// <summary>
        /// 스크린 해상도
        /// </summary>
        private Vector2 screenSize;
        public Vector2 ScreenSize => this.screenSize;

        /// <summary>
        /// 해상도 비율(가로크기 / 세로크기)
        /// </summary>
        private float screenRatio = 0f;
        public float ScreenRatio => this.screenRatio;

        /// <summary>
        /// 스크린해상도 설정이 제대로 됐는지 확인
        /// </summary>
        public bool IsCompleteScreenInformation => (screenRatio > 0f);

        /// <summary>
        /// 현재 TouchCenter 카메라 옵션을 고려한 터치 관련 파라미터 값 설정
        /// </summary>
        private void SettingScreenInformation()
        {
            screenSize = new Vector2(Screen.width, Screen.height);
            screenRatio = screenSize.x / screenSize.y;

#if UNITY_EDITOR
            if (MainTouchCenter == null)
                Debug.LogWarning("메인 터치 카메라가 설정되기 전에 들어옴, 그래서 5로 박아서 설정");
#endif
            unityPositionPerOnePixel = OrthographicSize / (screenSize.y * 0.5f);

#if false
            if(MainTouchCenter != null)
                Debug.LogFormat("orthographicSize : {0}, screenSize.y : {1}, UPPOP : {2}"
                , MainTouchCenter.MainCam.orthographicSize, screenSize.y, worldPos_perOnePixel);
#endif
        }

        #region Touch
        // 유효하지 않은 터치 포지션
        public readonly Vector2 InvalidTouchPosition = Vector2.zero;

        /// <summary>
        /// 터치 포지션 유효성 확인
        /// </summary>
        public bool ValidateTouchPosition(Vector2 position)
        {
            return !position.Equals(InvalidTouchPosition);
        }

        /// <summary>
        /// fingerId를 기반으로 터치의 위치값을 반환
        /// </summary>
        public Vector2 GetTouchPosition(int fingerId)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId == fingerId)
                    return Input.touches[i].position;
            }

#if CONSIDER_MOUSE
            switch (fingerId)
            {
                case MouseIdLeft:
                    if (Input.GetMouseButton(0))
                        return Input.mousePosition;
                    else
                        break;
                case MouseIdRight:
                    if (Input.GetMouseButton(1))
                        return Input.mousePosition;
                    else
                        break;
            }
#endif

            return InvalidTouchPosition;
        }

#if CONSIDER_MOUSE
        public bool IsMouse(int fingerId)
        {
            return fingerId == MouseIdLeft || fingerId == MouseIdRight;
        }
#endif
        #endregion

        #region Collision
        /// <summary>
        /// [메인 카메라 기준] Collider2D 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent2D(int fingerId)
        {
            Vector2 pos = GetTouchPosition(fingerId);

            if (ValidateTouchPosition(pos))
                return GetTouchedComponent2D(pos);
            else
                return null;
        }

        /// <summary>
        /// [메인 카메라 기준] Collider2D 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent2D(Vector2 position)
        {
            return GetTouchedComponent2D(MainTouchCenter.MainCam, position);
        }

        /// <summary>
        /// Collider2D 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent2D(Camera camera, Vector2 position)
        {
            Vector2 touchedPosition = camera.ScreenToWorldPoint(position);
            Ray2D ray = new Ray2D(touchedPosition, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            return hit.collider;
        }

        /// <summary>
        /// [메인 카메라 기준] Collider(3D) 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent3D(int fingerId)
        {
            Vector2 pos = GetTouchPosition(fingerId);

            if (ValidateTouchPosition(pos))
                return GetTouchedComponent3D(pos);
            else
                return null;
        }

        /// <summary>
        /// [메인 카메라 기준] Collider(3D) 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent3D(Vector2 position)
        {
            return GetTouchedComponent3D(MainTouchCenter.MainCam, position);
        }

        /// <summary>
        /// Collider(3D) 터치 충돌 컴포넌트 반환
        /// </summary>
        public Component GetTouchedComponent3D(Camera camera, Vector2 position)
        {
            Ray ray = camera.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                return hit.collider;
            else
                return null;
        }
        #endregion

        #region Convert
        /// <summary>
        /// 스크린(touch) 위치값을 유니티 위치값으로 변환
        /// </summary>
        public Vector2 ConvertUnityPosition(Vector2 position)
        {
            Vector2 halfScreenSize = ScreenSize * 0.5f;

            // 여기에 픽셀 기준(0, 0)값이 서로 다르니 보정하여 계산
            /// 유니티는 기준이 화면 중앙이고 스크린은 좌측하단.
            /// 하여 서로간의 위치 차이인 스크린 절반크기값(halfScreenSize)를 각각 빼주어 계산
            position.x = (position.x - halfScreenSize.x) * UPPOP;
            position.y = (position.y - halfScreenSize.y) * UPPOP;
            return position;
        }

        /// <summary>
        /// 유니티 위치값을 스크린(touch) 위치값으로 변환
        /// </summary>
        public Vector2 ConvertScreenPosition(Vector2 position)
        {
            Vector2 halfScreenSize = ScreenSize * 0.5f;
            // 유니티 크기(Unit) 1당 스크린(touch) 크기 계산
            /// 유니티 카메라는 기본적으로 y축을 기준으로 감
            /// y축을 기준 전체 유니티 크기값은 OrthographicSize * 2
            ///  - 예를들어 OrthographicSize 5의 경우 화면 기준 유니티 크기값은 10이된다.
            ///  - 다르게 말해 y축 기준 유니티 크기의 절반이 OrthographicSize인 것이다.
            /// 하여 스크린 전체를 OrthographicSize * 2로 나누면 유니티 크기 1당 스크린 크기가 계산된다.
            float screenPerOneUnit = ScreenSize.y / (OrthographicSize * 2f);

            // 여기에 픽셀 기준(0, 0)값이 서로 다르니 보정
            /// 유니티는 기준이 화면 중앙이고 스크린은 좌측하단.
            /// 하여 서로간의 위치 차이인 스크린 절반크기값(halfScreenSize)를 각각 더해준다.
            position.x = (position.x * screenPerOneUnit) + halfScreenSize.x;
            position.y = (position.y * screenPerOneUnit) + halfScreenSize.y;

            return position;
        }

        /// <summary>
        /// 현재 화면의 Unity 크기 기준 영역값 반환
        /// </summary>
        public Vector2 GetUnitArea()
        {
            float orthographicSize = OrthographicSize;

            /// 현재 화면영역은 -return ~ return의 Unity 크기를 가진다.
            /// -return.x ~ return.x
            /// -return.y ~ return.y
            return new Vector2(orthographicSize * screenRatio, orthographicSize);
        }
        #endregion

        #region TouchEventSystem
        /// <summary>
        /// 터치 이벤트 시스템
        /// </summary>
        public readonly ITouchEventSystem EventSystem = null;
        #endregion
    }
}