using LibraProgramming.ChatRoom.Client.Services.Hessian;

namespace LibraProgramming.ChatRoom.Client.Services
{
    // http://mostoriginalsberlin.viceland.com/resin-doc/doc/hessian-2.0-spec.xtp
    public class ChatClient : ClientBase<ChatClient>
    {
        public ChatClient(Channel channel)
            : base(channel)
        {
        }
    }
}