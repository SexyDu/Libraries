using UnityEngine;

namespace SexyDu.Pattern.Behavioral.State.UseAbstractClass
{
    #region State
    /// <summary>
    /// 상태 부모 클래스
    /// </summary>
    public abstract class State
    {
        // 다음 스탭 처리 함수
        public abstract void Handle(Context context);
        // 상태값
        public abstract StateType Type { get; }
    }

    /// <summary>
    /// A 상태
    /// </summary>
    public class StateTypeA : State
    {
        public override void Handle(Context context)
        {
            context.SetState(new StateTypeB());
        }

        public override StateType Type => StateType.A;
    }

    /// <summary>
    /// B 상태
    /// </summary>
    public class StateTypeB : State
    {
        public override void Handle(Context context)
        {
            context.SetState(new StateTypeA());
        }

        public override StateType Type => StateType.B;
    }
    #endregion

    #region Context
    public class Context
    {
        private State state;

        public Context(State state)
        {
            SetState(state);
        }

        public void SetState(State state)
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