using UnityEngine;

namespace SexyDu.Pattern.Structural.Adapter
{
    /// Mover를 만들고 그를 상속받는 클래스들이 생긴다
    public abstract class Mover
    {
        public abstract void Going();
    }

    public class Car : Mover
    {
        public override void Going()
        {
            Debug.Log("Drive Car");
        }
    }

    public class Bike : Mover
    {
        public override void Going()
        {
            Debug.Log("Run Bike");
        }
    }

    /// 뜬금없이 Mover를 상속받지 않은 Human이 생긴다
    public class Human
    {
        public void Walk()
        {
            Debug.Log("Walk man");
        }
    }

    /// Human을 Mover를 상속받은 클래스와 동일하게 움직이도록 하기 위해 Adapter를 만들어서 연결한다
    public class HumanMoverAdapter : Mover
    {
        private Human human = null;

        public HumanMoverAdapter(Human human)
        {
            this.human = human;
        }

        public override void Going()
        {
            human.Walk();
        }
    }
}