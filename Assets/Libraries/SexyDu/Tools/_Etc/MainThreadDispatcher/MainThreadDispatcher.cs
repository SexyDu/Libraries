using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SexyDu.Tool;

namespace SexyDu.Tools
{
    /// <summary>
    /// Main Thread Dispatcher 기본 동작 클래스
    /// </summary>
    public class MainThreadDispatcher : IMainThreadDispatcher
    {
        // 메인 스레드에서 실행할 작업을 저장하는 큐
        protected readonly Queue<Action> _actions = new Queue<Action>();

        /// <summary>
        /// 큐에 저장된 작업을 메인 스레드에서 실행합니다.
        /// </summary>
        public virtual void Execute()
        {
            lock (_actions)
            {
                while (_actions.Count > 0)
                {
                    _actions.Dequeue()?.Invoke();
                }
            }
        }

        /// <summary>
        /// 작업을 큐에 추가합니다.
        /// </summary>
        /// <param name="action">메인 스레드에서 실행할 작업</param>
        public virtual void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        /// <summary>
        /// 코루틴을 큐에 추가합니다.
        /// </summary>
        /// <param name="coroutine">메인 스레드에서 실행할 코루틴</param>
        private void Enqueue(IEnumerator coroutine)
        {
            lock (_actions)
            {
                _actions.Enqueue(() =>
                {
                    MonoHelper.StartCoroutine(coroutine);
                });
            }
        }

        /// <summary>
        /// 작업을 큐에 추가합니다.
        /// </summary>
        /// <param name="action">메인 스레드에서 실행할 작업</param>
        public virtual Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            Enqueue(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// 메인 스레드에서 실행할 작업을 IEnerator로 감쌉니다.
        /// </summary>
        /// <param name="action">메인 스레드에서 실행할 작업</param>
        private IEnumerator ActionWrapper(Action action)
        {
            action();
            yield return null;
        }
    }
}
