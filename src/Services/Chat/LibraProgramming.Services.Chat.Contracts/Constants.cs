using System;

namespace LibraProgramming.Services.Chat.Contracts
{
    public static class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public static class Streams
        {
            public static readonly Guid ChatRooms = new Guid("42582746-4A24-4DF9-8F04-1EC20DEFE805");

            /// <summary>
            /// 
            /// </summary>
            public static class Namespaces
            {
                public const string Chats = nameof(Constants) + nameof(Streams) + nameof(Namespaces) + nameof(Chats);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Resolvers
        {
            public const long ChatRooms = 0L;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class StreamProviders
        {
            public const string ChatRooms = nameof(StreamProviders) + nameof(ChatRooms);
        }

        /// <summary>
        /// 
        /// </summary>
        public static class StorageProviders
        {
            public const string ChatRooms = nameof(StorageProviders) + nameof(ChatRooms);
        }
    }
}