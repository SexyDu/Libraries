using System;

namespace SexyDu.Touch
{
    public class TouchConfigSingleton
    {
        private static Lazy<TouchConfig> ins = new Lazy<TouchConfig>(() => new TouchConfig());
        public static TouchConfig Ins => ins.Value;
    }
}