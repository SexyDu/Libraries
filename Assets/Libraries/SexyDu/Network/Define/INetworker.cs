using System;

namespace SexyDu.Network
{
    public interface INetworker : IDisposable
    {
        /// <summary>
        /// 작업 중 여부
        /// </summary>
        public bool IsWorking { get; }
    }
}