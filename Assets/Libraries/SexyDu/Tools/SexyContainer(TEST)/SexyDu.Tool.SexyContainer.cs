//#define ONLY_CLASS

using System;
using System.Collections.Generic;

/// <summary>
/// Single Container System 테스트 코드
/// </summary>
namespace SexyDu.Tool
{
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

    public class SexyContainer : ISexyContainer
    {
        private static readonly Lazy<SexyContainer> ins = new Lazy<SexyContainer>(new SexyContainer());
        public static ISexyContainer Ins => ins.Value;

        public void Bind<T>(T data) where T : ISexyBaggage
        {
            Type key = typeof(T);
            if (ab.ContainsKey(key))
            {
                throw new AlreadyBindedBaggageException(key);
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
                // 동일한 타입으로의 Unboxing은 비용이 무시해도 될 수준으로 작기 때문에 효율적
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
                // 동일한 타입으로의 Unboxing은 비용이 무시해도 될 수준으로 작기 때문에 효율적
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

    public interface ISexyContainer
    {
        public void Bind<T>(T data) where T : ISexyBaggage;

        public void Unbind<T>() where T : ISexyBaggage;

#if ONLY_CLASS
        public T Get<T>() where T : class;
#else
        public T Get<T>() where T : ISexyBaggage;
#endif


        public bool Has<T>();

        public bool Has(Type key);
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