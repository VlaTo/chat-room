    using System;

namespace LibraProgramming.ChatRoom.Common.Core
{
    public abstract class Context<TState>
        where TState : IState
    {
        public TState State
        {
            get;
            protected set;
        }

        protected Context(TState state)
        {
            State = state;
        }
    }
}
