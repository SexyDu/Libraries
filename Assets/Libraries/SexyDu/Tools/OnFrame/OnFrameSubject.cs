using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Tool
{
    /// <summary>
    /// 매 프레임 동작 서브젝트
    /// </summary>
    public class OnFrameSubject : IOnFrameSubject
    {
        /// <summary>
        /// OnFrame 대상
        /// </summary>
        protected List<IOnFrameTarget> targets = new List<IOnFrameTarget>();
        private bool HasTarget => targets.Count > 0;

        /// <summary>
        /// 프래임 동작 대상 등록
        /// </summary>
        public void Subscribe(IOnFrameTarget target)
        {
            if (targets.Contains(target))
            {
                Debug.LogWarning("이미 등록된 타겟입니다.");
                return;
            }
            else
            {
                targets.Add(target);

                if (!IsRunning)
                    coroutine = MonoHelper.StartCoroutine(UpdateFrame());
            }
        }

        /// <summary>
        /// 프래임 동작 대상 해제
        /// </summary>
        public void Unsubscribe(IOnFrameTarget target)
        {
            if (targets.Contains(target))
            {
                targets.Remove(target);
                if (!HasTarget)
                {
                    coroutine.Dispose();
                    coroutine = null;
                }
            }
        }

        /// <summary>
        /// 프래임 동작 대상 전체 해제
        /// </summary>
        private void UnsubscribeAll()
        {
            Stop();

            targets.Clear();
        }

        /// <summary>
        /// 프래임 동작 클리어
        /// </summary>
        protected void Clear()
        {
            UnsubscribeAll();
        }

        // 코루틴 커맨더
        private CoroutineCommander coroutine = null;
        // 코루틴 동작 여부
        protected bool IsRunning => (coroutine != null && coroutine.IsRunning);

        /// <summary>
        /// UpdateFrame 실행
        /// </summary>
        private void Run()
        {
            if (!IsRunning)
                coroutine = MonoHelper.StartCoroutine(UpdateFrame());
        }

        /// <summary>
        /// UpdateFrame 종료
        /// </summary>
        private void Stop()
        {
            if (IsRunning)
            {
                coroutine.Dispose();
                coroutine = null;
            }
        }

        /// <summary>
        /// 프래임 동작 코루틴
        /// </summary>
        private IEnumerator UpdateFrame()
        {
            while (targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].OnFrame();
                }

                yield return null;
            }
        }
    }
}