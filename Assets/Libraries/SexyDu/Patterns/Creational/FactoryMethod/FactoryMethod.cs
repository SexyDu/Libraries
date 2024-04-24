/// 팩토리 메서드 패턴
/// - 팩토리의 업그레이드 버전
/// - 클래스의 생성 형태에 따라 여러 팩토리를 구성하는 방식

using UnityEngine;

namespace SexyDu.Pattern.Creational.FactoryMethod
{
    #region Object
    public abstract class Based
    {
        public abstract void SomeFunction();

        protected bool locked = false;
        public void SetLockState(bool locked)
        {
            this.locked = locked;
        }

        protected int price = 0;
        public void SetDoublePrice()
        {
            price *= 2;
        }
    }

    public class ClassTypeA : Based
    {
        public ClassTypeA()
        {
            this.price = 100;
        }

        public override void SomeFunction()
        {
            Debug.Log("TypeAClass SomeFunction");
        }
    }

    public class ClassTypeB : Based
    {
        public ClassTypeB()
        {
            this.price = 200;
        }

        public override void SomeFunction()
        {
            Debug.Log("TypeBClass SomeFunction");
        }
    }
    #endregion

    #region Factory
    // 기본 공장
    public abstract class BasedFactory
    {
        public abstract Based MakeObject();
    }

    // lock 설정 A 생성 공장
    public class FactoryForLockA : BasedFactory
    {
        public override Based MakeObject()
        {
            ClassTypeA obj = new ClassTypeA();
            obj.SetLockState(true);

            return obj;
        }
    }

    // lock 설정 B 생성 공장
    public class FactoryForLockB : BasedFactory
    {
        public override Based MakeObject()
        {
            ClassTypeB obj = new ClassTypeB();
            obj.SetLockState(true);

            return obj;
        }
    }

    // 2배 가격 A 생성 공장
    public class FactoryForDoublePriceA : BasedFactory
    {
        public override Based MakeObject()
        {
            ClassTypeA obj = new ClassTypeA();
            obj.SetDoublePrice();

            return obj;
        }
    }

    // 2배 가격 B 생성 공장
    public class FactoryForDoublePriceB : BasedFactory
    {
        public override Based MakeObject()
        {
            ClassTypeB obj = new ClassTypeB();
            obj.SetDoublePrice();

            return obj;
        }
    }
    #endregion
}