using System.Collections.Generic;
using UnityEngine.Networking;

namespace SexyDu
{
    namespace Network
    {
        /// <summary>
        /// REST API 요청 접수증 기본 인터페이스
        /// </summary>
        public interface IRESTReceipt
        {
            // 요청 URL
            public string Url { get; }
            // 요청 형식
            public RESTMethod Method { get; }
            // 요청 타임아웃
            public int Timeout { get; }
            // 요청 헤더
            public Dictionary<string, string> Headers { get; }
        }

        /// <summary>
        /// Post 데이터를 포함한 REST API 요청 접수증
        /// Methos POST, PATCH, DELETE에 사용 가능
        /// </summary>
        public interface IPostableRESTReceipt : IRESTReceipt
        {
            // 요청 포스트 데이터
            public string PostData { get; }
        }

        /// <summary>
        /// REST API 요청 메소드 타입
        /// </summary>
        public enum RESTMethod : byte
        {
            GET = 0,
            POST,
            PATCH,
            DELETE
        }

        /// <summary>
        /// REST 수신 결과 타입
        /// * 해당 내용은 UntiyWebRequest.Result와 동일
        /// </summary>
        public enum RESTResult : byte
        {
            //
            // 요약:
            //     The request hasn't finished yet.
            InProgress,
            //
            // 요약:
            //     The request succeeded.
            Success,
            //
            // 요약:
            //     Failed to communicate with the server. For example, the request couldn't connect
            //     or it could not establish a secure channel.
            ConnectionError,
            //
            // 요약:
            //     The server returned an error response. The request succeeded in communicating
            //     with the server, but received an error as defined by the connection protocol.
            ProtocolError,
            //
            // 요약:
            //     Error processing data. The request succeeded in communicating with the server,
            //     but encountered an error when processing the received data. For example, the
            //     data was corrupted or not in the correct format.
            DataProcessingError
        }

        /// <summary>
        /// REST 수신 데이터 인터페이스
        /// </summary>
        public interface IRESTResponse
        {
            // 수신 코드
            public long code { get; }
            // 수신 문자열
            public string text { get; }
            // 수신 에러
            public string error { get; }
            // 수신 결과
            public RESTResult result { get; }
            // 수신 헤더 (딕셔너리)
            public Dictionary<string, string> headers { get; }
            
            /// <summary>
            /// REST API 성공 여부
            /// </summary>
            public bool IsSuccess => result == RESTResult.Success;
            /// <summary>
            /// Empty 여부
            /// </summary>
            /// <returns></returns>
            public bool IsEmpty => code == long.MinValue;
            /// <summary>
            /// InternalServerError 여부
            /// </summary>
            public bool IsInternalServerError => code == 500;
        }

        /// <summary>
        /// REST 수신 데이터
        /// </summary>
        public struct RESTResponse : IRESTResponse
        {
            // response code
            public readonly long code
            {
                get;
            }
            // 수신받은 Text 문자열 (json 등)
            public readonly string text
            {
                get;
            }
            // 에러 문자열
            public readonly string error
            {
                get;
            }
            // 결과
            public readonly RESTResult result
            {
                get;
            }
            // 헤더 정보
            public readonly Dictionary<string, string> headers
            {
                get;
            }

            public RESTResponse(long code, string text, string error, RESTResult result, Dictionary<string, string> headers = null)
            {
                this.code = code;
                this.text = text;
                this.error = error;
                this.result = result;
                this.headers = headers;
            }

            /// <summary>
            /// 빈 수신 데이터
            /// </summary>
            public static readonly RESTResponse Empty = new RESTResponse(long.MinValue, string.Empty, string.Empty, RESTResult.ConnectionError);
        }
    }
}