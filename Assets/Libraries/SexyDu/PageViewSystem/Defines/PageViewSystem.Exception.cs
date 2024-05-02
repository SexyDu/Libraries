using System;

namespace SexyDu.PageViewSystem
{
    /// <summary>
    /// 생성하려는 MonoPageView(Prefab)을 찾지 못한 경우의 Exception
    /// </summary>
    public class NotFoundPageViewException : Exception
    {
        private const string DefaultExceptionMessage = "NotFoundPageViewException : Not found 'MonoPageView' resource [Resource.Load]";

        public NotFoundPageViewException() : base(DefaultExceptionMessage) { }

        public NotFoundPageViewException(string message) : base(message) { }
    }
}