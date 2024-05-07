using UnityEngine;

namespace SexyDu.Pattern.Behavioral.State.UseInterface
{
    #region State
    /// <summary>
    /// 상태 기본 인터페이스
    /// </summary>
    public interface IState
    {
        // 다음 스탭 처리 함수
        public void Handle(IContext context);
        // 상태값
        public StateType Type { get; }
    }

    /// <summary>
    /// A 상태
    /// </summary>
    public struct StateTypeA : IState
    {
        public void Handle(IContext context)
        {
            context.SetState(new StateTypeB());
        }

        public StateType Type => StateType.A;
    }

    /// <summary>
    /// B 상태
    /// </summary>
    public struct StateTypeB : IState
    {
        public void Handle(IContext context)
        {
            context.SetState(new StateTypeA());
        }

        public StateType Type => StateType.B;
    }
    #endregion

    #region Context
    public interface IContext
    {
        public void SetState(IState state);
    }

    public class Context : IContext
    {
        private IState state;

        public Context(IState state)
        {
            SetState(state);
        }

        public void SetState(IState state)
        {
            this.state = state;

            Debug.LogFormat("상태 변경 : {0}", state.Type);
        }

        public void Request()
        {
            this.state.Handle(this);
        }
    }
    #endregion
}