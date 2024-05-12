using System;
using UnityEngine;

namespace SexyDu.Tool
{
    /// <summary>
    /// MonoHelper 클래스를 통해 생성된 코루틴 오브젝트
    /// </summary>
    public class HelperCoroutine : IDisposable
    {
        private Coroutine co = null;
        public Coroutine Co { get { return co; } }

        public HelperCoroutine(Coroutine co)
        {
            this.co = co;
        }

        public void Dispose()
        {
            MonoHelper.StopCoroutine(co);
            co = null;
        }
    }
}