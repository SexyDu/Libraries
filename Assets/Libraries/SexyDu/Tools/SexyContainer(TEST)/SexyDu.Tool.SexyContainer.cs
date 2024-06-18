//#define ONLY_CLASS

using System;
using System.Collections.Generic;

namespace SexyDu.Tool
{
    public class SexyContainer
    {
        private static readonly Lazy<SexyContainer> ins = new Lazy<SexyContainer>(new SexyContainer());
        public static SexyContainer Ins => ins.Value;

        public void Bind<T>(T data) where T : ISexyBaggage
        {
            Type key = typeof(T);
            if (ab.ContainsKey(key))
            {

            }
            else
                ab.Add(typeof(T), data);
        }

        public void Unbind<T>() where T : ISexyBaggage
        {
            ab.Remove(typeof(T));
        }

#if ONLY_CLASS
        public T Get<T>() where T : class
        {
            Type key = typeof(T);
            if (ab.ContainsKey(key))
            {
                return ab[key] as T;
            }
            else
                return null;
        }
#else
        public T Get<T>() where T : ISexyBaggage
        {
            Type key = typeof(T);
            if (Has(key))
            {
                return (T)ab[key];
            }
            else
                return default(T);
        }
#endif


        public bool Has<T>()
        {
            return ab.ContainsKey(typeof(T));
        }

        public bool Has(Type key)
        {
            return ab.ContainsKey(key);
        }

        //private Dictionary<Type, Capsule<T>> ab = new Dictionary<Type, Capsule<T>();
        private Dictionary<Type, ISexyBaggage> ab = new Dictionary<Type, ISexyBaggage>();
        
    }

    public interface ISexyBaggage
    {
    }

    public class TestBaggage : ISexyBaggage
    {
        public void Bind()
        {
            SexyContainer.Ins.Bind(this);
        }

        public void Unbind()
        {
            SexyContainer.Ins.Unbind<TestBaggage>();
        }

        private void Use()
        {
            SexyContainer.Ins.Get<TestBaggage>().a++;
        }

        public int a;
    }

#if false
    public struct Baggage
    {
        private List<IBaggageUser> users;

        private readonly object content;
        private readonly Type type;

        public Baggage(object content)
        {
            this.type = content.GetType();
            this.content = content;
            this.users = null;
        }

        public T Get<T>()
        {
            if (type.Equals(typeof(T)))
            {
                if (users != null && users.Count > 0)
                {
                    return users[0].GetContent<T>();
                }
                else
                {
                    return (T)content;
                }
            }
            else
                return default(T);
        }

        public T Inject<T>(IBaggageUser user)
        {
            if (users == null)
            {
                users = new List<IBaggageUser>();
            }

            users.Add(user);

            return Get<T>();
        }
    }

    public interface IBaggageUser
    {
        public T GetContent<T>();
    }

    public class Capsule<T> where T : class
    {
        public Capsule(T data)
        {

        }

        private readonly T data = null;
        public T Data => data;
    }
#endif

}