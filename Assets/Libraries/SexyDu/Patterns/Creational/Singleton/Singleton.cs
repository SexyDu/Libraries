using System;

namespace SexyDu.Pattern.Creational.Singleton
{
    public class Singleton
    {
        private static Lazy<Singleton> instance = new Lazy<Singleton>(() => new Singleton());

        public static Singleton Instance { get { return instance.Value; } }

        public Singleton()
        {

        }
    }
}