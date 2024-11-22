using System;
using System.Text;
using System.Collections.Generic;

namespace SexyDu.Network.Editor
{
    /// <summary>
    /// 수신 베이스
    /// </summary>
    [Serializable]
    public class BaseResponse : IClearable
    {
        // 코드
        public long code = long.MinValue;
        // 에러
        public string error = string.Empty;
        // 결과
        public NetworkResult result = NetworkResult.Unknown;
        // 헤더 (딕셔너리)
        public Dictionary<string, string> headers = null;


        // 비어있는 데이터인지 여부
        public bool IsEmpty => result == NetworkResult.Unknown;
        // 성공 여부
        public bool IsSuccess => result == NetworkResult.Success;

        /// <summary>
        /// 수신 데이터 저장
        /// </summary>
        /// <param name="response">수신 데이터</param>
        protected virtual void Set(IResponse response)
        {
            code = response.code;
            error = response.error;
            result = response.result;
            headers = response.headers;

            RecodeResponseTime();
        }

        /// <summary>
        /// 클리어
        /// </summary>
        public virtual void Clear()
        {
            code = long.MinValue;
            error = string.Empty;
            result = NetworkResult.Unknown;
            headers = null;

            responseTime = 0;
        }

        #region Recode pass time
        // 수신 소요시간
        public double responseTime = 0;
        // 요청 DateTime
        private DateTime requestDateTime = DateTime.MinValue;
        /// <summary>
        /// 요청 DateTime 기록
        /// </summary>
        public void RecordReqeustTime()
        {
            requestDateTime = DateTime.Now;
        }
        /// <summary>
        /// 수신 시간 기록
        /// </summary>
        protected void RecodeResponseTime()
        {
            if (requestDateTime != DateTime.MinValue)
            {
                TimeSpan difference = DateTime.Now - requestDateTime;
                responseTime = difference.TotalSeconds;
            }
            else
                responseTime = 0;
        }
        #endregion

        /// <summary>
        /// 오브젝트 문자화
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Response").AppendLine();
            sb.AppendFormat("- code : {0}", code).AppendLine();
            sb.AppendFormat("- error : {0}", error).AppendLine();
            sb.AppendFormat("- result : {0}", result);
            if(headers != null && headers.Count > 0)
            {
                sb.AppendLine();
                sb.Append("- headers");
                foreach (var header in headers)
                {
                    sb.AppendLine();
                    sb.AppendFormat("    \"{0}\": \"{1}\"", header.Key, header.Value);
                }
            }

            return sb.ToString();
        }
    }
}