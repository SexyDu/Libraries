using System.Collections.Generic;

namespace SexyDu
{
    namespace Network
    {
        /// <summary>
        /// REST API 요청 메소드 타입
        /// </summary>
        public enum NetworkMethod : byte
        {
            GET = 0,
            POST,
            PATCH,
            DELETE
        }

        /// <summary>
        /// REST 수신 결과 타입
        /// * 해당 내용은 UntiyWebRequest.Result 참조
        /// </summary>
        public enum NetworkResult : byte
        {
            Unknown = 0,

            /// <UnityWebRequest.Result>
            /// UnityWebRequest.Result 동일 영역
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
            DataProcessingError,
            /// </UnityWebRequest.Result>
            /// 

            SuccessFromCache,
            
            // 인증 토큰이 유효하지 않은 경우
            /// 아직 토큰에 대해 고려하지 않기 때문에 일단 주석
            // InvalidAuthentionToken,
        }
    }
}