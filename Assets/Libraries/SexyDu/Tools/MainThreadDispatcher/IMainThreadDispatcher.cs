using System;
using System.Threading.Tasks;

namespace SexyDu.Tools
{   
    /// <summary>
    /// 메인 스레드에서 실행할 작업을 큐에 추가하는 인터페이스
    /// </summary>
    public interface IMainThreadDispatcher
    {
        /// <summary>
        /// 메인 스레드에서 실행할 작업을 큐에 추가합니다.
        /// </summary>
        /// <param name="action">메인 스레드에서 실행할 작업</param>
        void Enqueue(Action action);

        /// <summary>
        /// 메인 스레드에서 실행할 작업을 큐에 추가합니다.
        /// </summary>
        /// <param name="action">메인 스레드에서 실행할 작업</param>
        Task EnqueueAsync(Action action);
    }
}