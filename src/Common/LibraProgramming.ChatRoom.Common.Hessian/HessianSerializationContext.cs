using System;
using System.Collections;
using System.Collections.Generic;

namespace LibraProgramming.ChatRoom.Common.Hessian
{
    /// <summary>
    /// 
    /// </summary>
    public class HessianSerializationContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<Type> Classes
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList Instances
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public HessianSerializationContext()
        {
            Classes = new List<Type>();
            Instances = new List<object>();
        }
    }
}