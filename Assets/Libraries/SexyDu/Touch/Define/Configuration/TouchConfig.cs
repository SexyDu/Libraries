using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Touch
{
    public class TouchConfig
    {
        public TouchConfig()
        {

        }

        // 활성화된 전체 터치 센터
        private List<TouchCenter> touchCenters = new List<TouchCenter>();
        // 프로젝트 기본 OrthographicSize
        private const float DefaultOrthographicSize = 5f;

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

        /// <summary>
        /// 1픽셀당 유니티 오브젝트 크기/위치값
        /// </summary>
        private float worldPosPerOnePixel;
        public float WorldPosPerOnePixel
        {
            get { return this.worldPosPerOnePixel; }
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

            if (MainTouchCenter != null)
                worldPosPerOnePixel = MainTouchCenter.MainCam.orthographicSize / (screenSize.y * 0.5f);
            else
            {
                Debug.LogWarning("메인 터치 카메라가 설정되기 전에 들어옴, 그래서 5로 박아서 설정");
                worldPosPerOnePixel = DefaultOrthographicSize / (screenSize.y * 0.5f);
            }

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
                case TouchCenter.MouseIdLeft:
                    if (Input.GetMouseButton(0))
                        return Input.mousePosition;
                    else
                        break;
                case TouchCenter.MouseIdRight:
                    if (Input.GetMouseButton(1))
                        return Input.mousePosition;
                    else
                        break;
            }
#endif

            return InvalidTouchPosition;
        }
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

    }
}