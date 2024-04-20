/// 팩토리 패턴
/// - 특정 클래스를 상속 받은 여러 유형의 클래스를 공장처럼 찍어낼 수 있는 형태
/// - 클래스형 팩토리패턴과 함수형 팩토리 패턴이 있다.
/// - 아래 예시는 클래스형 팩토리 패턴이다.

using UnityEngine;

namespace SexyDu.Pattern.Factory
{
    #region Object
    public abstract class Based
    {
        public abstract void SomeFunction();
    }

    public class ClassTypeA : Based
    {
        public override void SomeFunction()
        {
            Debug.Log("TypeAClass SomeFunction");
        }
    }

    public class ClassTypeB : Based
    {
        public override void SomeFunction()
        {
            Debug.Log("TypeBClass SomeFunction");
        }
    }
    #endregion

    #region Factory
    public class FactoryClass
    {
        public Based MakeClass(FactoryType type)
        {
            switch (type)
            {
                case FactoryType.A:
                    return new ClassTypeA();
                case FactoryType.B:
                    return new ClassTypeB();
                default:
                    return null;
            }
        }
    }

    public enum FactoryType : byte
    {
        Unknown = 0,
        A,
        B,
    }
    #endregion
}