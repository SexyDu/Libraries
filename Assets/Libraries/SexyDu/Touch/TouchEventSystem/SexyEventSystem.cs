#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
#define CONSIDER_MOUSE
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using SexyDu.Tool;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SexyDu.Touch
{
    /// <summary>
    /// 유니티 터치 이벤트 시스템
    ///  * 현재는 Begin 이벤트만 처리한다.
    /// </summary>
    public class SexyEventSystem : ITouchEventSystem
    {
        // 터치 수신자 리스트
        private readonly List<ITouchEventReceiver> receivers = new List<ITouchEventReceiver>();

        /// <summary>
        /// 터치 수신자 등록
        /// </summary>
        public void Subscribe(ITouchEventReceiver receiver)
        {
            receivers.Add(receiver);

            if (!IsUpdating)
                Run();
        }
        /// <summary>
        /// 터치 수신자 제거
        /// </summary>
        public void Unsubscribe(ITouchEventReceiver receiver)
        {
            receivers.Remove(receiver);

            if (receivers.Count == 0)
                Stop();
        }
        /// <summary>
        /// 터치 수신자 클리어
        /// </summary>
        public void ClearSubscription()
        {
            receivers.Clear();

            Stop();
        }
        /// <summary>
        /// 터치 이벤트 전송
        /// </summary>
        public void SendTouch(UnityEngine.Touch touch)
        {
            foreach (var receiver in receivers)
                receiver.OnTouchBegin(touch);
        }
        /// <summary>
        /// 마우스 이벤트 전송
        /// </summary>
        public void SendMouse(int mouseId, Vector2 position)
        {
            foreach (var receiver in receivers)
                receiver.OnMouseBegin(mouseId, position);
        }

        #region Update
        private IDisposable update = null;
        private bool IsUpdating => update != null;
        /// <summary>
        /// 업데이트 시작
        /// </summary>
        private void Run()
        {
            update = MonoHelper.StartCoroutine(CoUpdate());
        }
        /// <summary>
        /// 업데이트 종료
        /// </summary>
        private void Stop()
        {
            if (IsUpdating)
            {
                update.Dispose();
                update = null;
            }
        }
        /// <summary>
        /// 업데이트 코루틴
        /// </summary>
        private IEnumerator CoUpdate()
        {
            do
            {
                yield return null;

#if CONSIDER_MOUSE
                bool hasBeginTouch = false;
#endif
                for (int i = 0; i < Input.touchCount; i++)
                {
                    // 터치 시작 상태이고 캔버스 영역이 아니면
                    if (Input.touches[i].phase.Equals(TouchPhase.Began) && !IsCanvasTouch(Input.touches[i]))
                    {
                        SendTouch(Input.touches[i]);
#if CONSIDER_MOUSE
                        hasBeginTouch = true;
#endif
                    }
                }

#if CONSIDER_MOUSE
                // 터치 시작 이벤트가 없거나 마우스가 캔버스 영역이 아니면
                if (!hasBeginTouch || !IsCanvasMouse())
                {
                    if (Input.GetMouseButtonDown(0))
                        SendMouse(TouchConfig.MouseIdLeft, Input.mousePosition);
                    else if (Input.GetMouseButtonDown(1))
                        SendMouse(TouchConfig.MouseIdRight, Input.mousePosition);
                }
#endif
            } while (true);
        }
        #endregion

        #region Canvas Check
        /// <summary>
        /// 해당 터치가 캔버스 영역(UGUI)에 있는지 반환
        /// </summary>
        private bool IsCanvasTouch(UnityEngine.Touch touch)
        {
            return EventSystem.current is null ? false : EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }

#if CONSIDER_MOUSE
        /// <summary>
        /// 마우스가 캔버스 영역(UGUI)에 있는지 반환
        /// </summary>
        private bool IsCanvasMouse()
        {
            return EventSystem.current is null ? false : EventSystem.current.IsPointerOverGameObject();
        }
#endif
        #endregion

    }
}