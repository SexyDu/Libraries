using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace SexyDu.Network
{
    /// <summary>
    /// UnityWebRequest를 사용하는 기반 네트워커 클래스
    /// </summary>
    public abstract class UnityNetworker : INetworker, IDisposable
    {
        /// <summary>
        /// 작업 중 여부
        /// </summary>
        public abstract bool IsWorking { get; }

        /// <summary>
        /// 해제
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 작업 종료
        /// </summary>
        protected virtual void Terminate()
        {
            Dispose();

            UnityEngine.Debug.Log("UnityNetworker Terminate");
        }

        /// <summary>
        /// 타임아웃 설정
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeout"></param>
        protected void SetTimeout(UnityWebRequest target, int timeout)
        {
            if (timeout > 0)
                target.timeout = timeout;
        }

        /// <summary>
        /// 요청 헤더 설정
        /// </summary>
        protected void SetRequestHeaders(UnityWebRequest target, Dictionary<string, string> headers)
        {
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    target.SetRequestHeader(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// UnityWebRequest.Result를 RESTResult 형식으로 변환
        /// </summary>
        protected virtual NetworkResult ToRESTResult(UnityWebRequest.Result result)
        {
            switch (result)
            {
                case UnityWebRequest.Result.InProgress:
                    return NetworkResult.InProgress;
                case UnityWebRequest.Result.Success:
                    return NetworkResult.Success;
                case UnityWebRequest.Result.ConnectionError:
                    return NetworkResult.ConnectionError;
                case UnityWebRequest.Result.ProtocolError:
                    return NetworkResult.ProtocolError;
                case UnityWebRequest.Result.DataProcessingError:
                    return NetworkResult.DataProcessingError;
                default:
                    return NetworkResult.Unknown;
            }
        }
    }
}