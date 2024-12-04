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
    public class TouchEventSystem : ITouchEventSystem
    {
        private readonly List<ITouchEventReceiver> receivers = new List<ITouchEventReceiver>();

        public void SendTouch(UnityEngine.Touch touch)
        {
            foreach (var receiver in receivers)
                receiver.OnTouchBegin(touch);
        }

        public void SendMouse(int mouseId, Vector2 position)
        {
            foreach (var receiver in receivers)
                receiver.OnMouseBegin(mouseId, position);
        }

        public void Subscribe(ITouchEventReceiver receiver)
        {
            receivers.Add(receiver);

            if (!IsUpdating)
                Run();
        }

        public void Unsubscribe(ITouchEventReceiver receiver)
        {
            receivers.Remove(receiver);

            if (receivers.Count == 0)
                Stop();
        }

        public void ClearSubscription()
        {
            receivers.Clear();

            Stop();
        }

        private IDisposable update = null;
        private bool IsUpdating => update != null;

        private void Run()
        {
            update = MonoHelper.StartCoroutine(CoUpdate());
        }

        private void Stop()
        {
            if (IsUpdating)
            {
                update.Dispose();
                update = null;
            }
        }

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
                    if (Input.touches[i].phase.Equals(TouchPhase.Began) // 터치 시작 상태이고
                     && !IsCanvasTouch(Input.touches[i])) // 캔버스 영역이 아니면
                    {
                        SendTouch(Input.touches[i]);
#if CONSIDER_MOUSE
                        hasBeginTouch = true;
#endif
                    }
                }

#if CONSIDER_MOUSE
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

        protected EventSystem eventSystem => EventSystem.current;

        /// <summary>
        /// 해당 터치가 캔버스 영역(UGUI)에 있는지 반환
        /// </summary>
        private bool IsCanvasTouch(UnityEngine.Touch touch)
        {
            return eventSystem is null ? false : eventSystem.IsPointerOverGameObject(touch.fingerId);
        }

#if CONSIDER_MOUSE
        /// <summary>
        /// 마우스가 캔버스 영역(UGUI)에 있는지 반환
        /// </summary>
        private bool IsCanvasMouse()
        {
            return eventSystem is null ? false : eventSystem.IsPointerOverGameObject();
        }
#endif
    }
}