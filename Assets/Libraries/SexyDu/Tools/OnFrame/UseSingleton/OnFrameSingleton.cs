using System;

namespace SexyDu.Tool
{
    public class OnFrameSingleton
    {
        private static Lazy<IOnFrameSubject> ins = new Lazy<IOnFrameSubject>(() => new OnFrameSubject());
        public static IOnFrameSubject Ins => ins.Value;
    }
}