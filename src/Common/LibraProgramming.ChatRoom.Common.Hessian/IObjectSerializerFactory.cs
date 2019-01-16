using System;

namespace LibraProgramming.ChatRoom.Common.Hessian
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObjectSerializerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        IObjectSerializer GetSerializer(Type target);
    }
}