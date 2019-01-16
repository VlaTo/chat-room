using System;
using System.Collections.Generic;

namespace LibraProgramming.ChatRoom.Common.Hessian
{
    internal sealed partial class HessianObjectSerializerFactory : IObjectSerializerFactory
    {
        private Dictionary<Type, IObjectSerializer> cache;

        /// <inheritdoc cref="IObjectSerializerFactory.GetSerializer" />
        public IObjectSerializer GetSerializer(Type target)
        {
            EnsureCache();
            return cache.TryGetValue(target, out var writer) ? writer : null;
        }

        private void EnsureCache()
        {
            if (null != cache)
            {
                return;
            }

            var dict = new Dictionary<Type, IObjectSerializer>
            {
                [typeof (bool)] = new BooleanSerializer(),
                [typeof (int)] = new Int32Serializer(),
                [typeof (long)] = new Int64Serializer(),
                [typeof (double)] = new DoubleSerializer(),
                [typeof (string)] = new StringSerializer(),
                [typeof (DateTime)] = new DateTimeSerializer()
            };
            
            cache = dict;
        }
    }
}