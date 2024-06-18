using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Tool
{
    public sealed class OnFrameContainer : IOnFrameContainer
    {
        private static Lazy<OnFrameContainer> ins = new Lazy<OnFrameContainer>(new OnFrameContainer());
        public static IOnFrameContainer Ins => ins.Value;

        private List<IOnFrameTarget> targets = new List<IOnFrameTarget>();
        private bool HasTargets => targets.Count > 0;

        public void Bind(IOnFrameTarget target)
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

        public void Unbind(IOnFrameTarget target)
        {
            if (targets.Contains(target))
            {
                targets.Remove(target);
                if (!HasTargets)
                {
                    coroutine.Dispose();
                    coroutine = null;
                }
            }
        }

        private CoroutineCommander coroutine = null;
        private bool IsRunning => (coroutine != null && coroutine.IsRunning);
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