using System;

namespace LibraProgramming.ChatRoom.Client.Services.Hessian
{
    public abstract class ClientBase
    {
        public ClientBase(Channel channel)
        {
            if (null == channel)
            {
                throw new ArgumentNullException(nameof(channel));
            }

        }
    }

    public class ClientBase<T> : ClientBase
        where T : ClientBase<T>
    {
        public ClientBase(Channel channel)
            : base(channel)
        {
        }
    }
}