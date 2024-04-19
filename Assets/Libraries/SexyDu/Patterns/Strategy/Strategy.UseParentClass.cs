using UnityEngine;

namespace SexyDu.Pattern.Strategy.UseParentClass
{
    public abstract class BasedClass
    {
        public abstract void SomeAction();
    }

    public class TypeAClass : BasedClass
    {
        public override void SomeAction()
        {
            Debug.LogFormat("Act Type A");
        }
    }

    public class TypeBClass : BasedClass
    {
        public override void SomeAction()
        {
            Debug.LogFormat("Act Type B");
        }
    }

    public class ExecutionClass
    {
        public void ActionSome(BasedClass target)
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