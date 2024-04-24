using UnityEngine;

namespace SexyDu.Pattern.Behavioral.Command
{
    /// <summary>
    /// 커맨드 베이스 클래스
    /// </summary>
    public abstract class Command
    {
        public abstract void Execute();
    }

    #region Apple형 커맨드
    public class CommandForApple : Command
    {
        private readonly Apple apple = null;

        public CommandForApple(Apple apple)
        {
            this.apple = apple;
        }

        public override void Execute()
        {
            apple.SliceApple();
        }
    }

    public class Apple
    {
        public Apple()
        {

        }

        public void SliceApple()
        {
            Debug.LogFormat("Slice apple");
        }
    }
    #endregion

    #region Banana형 커맨드
    public class CommandForBanana : Command
    {
        private readonly Banana banana = null;

        public CommandForBanana(Banana banana)
        {
            this.banana = banana;
        }

        public override void Execute()
        {
            banana.NakedBanana();
        }
    }

    public class Banana
    {
        public Banana()
        {

        }

        public void NakedBanana()
        {
            Debug.LogFormat("Naked banana");
        }
    }
    #endregion
}