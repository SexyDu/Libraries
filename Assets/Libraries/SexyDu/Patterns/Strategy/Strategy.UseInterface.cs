using UnityEngine;

namespace SexyDu.Pattern.Strategy.UseInterface
{
    public interface IBasedType
    {
        public void SomeAction();
    }

    public class TypeAClass : IBasedType
    {
        public void SomeAction()
        {
            Debug.LogFormat("Act Type A");
        }
    }

    public class TypeBClass : IBasedType
    {
        public void SomeAction()
        {
            Debug.LogFormat("Act Type B");
        }
    }

    public class ExecutionClass
    {
        public void ActionSome(IBasedType target)
        {
            target.SomeAction();
        }

        private void ActionTypeA()
        {
            ActionSome(new TypeAClass());
        }

        private void ActionTypeB()
        {
            ActionSome(new TypeBClass());
        }
    }
}