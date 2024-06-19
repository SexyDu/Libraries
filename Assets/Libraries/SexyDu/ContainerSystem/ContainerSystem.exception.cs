using System;

namespace SexyDu.ContainerSystem
{
    /// <summary>
    /// 컨테이너 도커에 컨데이터 연결 시 동일한 타입이 있는 경우 발생하는 Exception
    /// </summary>
    public class AlreadyDockedContainerException : Exception
    {
        public AlreadyDockedContainerException()
        {

        }

        private const string DefaultMessageFormat = "이미 연결된 타입({0})의 컨테이너입니다.";
        public AlreadyDockedContainerException(Type type) : base(string.Format(DefaultMessageFormat, type))
        {

        }

        public AlreadyDockedContainerException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// 컨테이너에 Baggage 적재 시 동일한 타입이 있는 경우 발생하는 Exception
    /// </summary>
    public class AlreadyBindedBaggageException : Exception
    {
        public AlreadyBindedBaggageException()
        {

        }

        private const string DefaultMessageFormat = "이미 적재된 타입({0})의 수하물입니다.";
        public AlreadyBindedBaggageException(Type type) : base(string.Format(DefaultMessageFormat, type))
        {

        }

        public AlreadyBindedBaggageException(string message) : base(message)
        {

        }
    }
}