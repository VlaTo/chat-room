using System;
using System.Reflection;
using System.Runtime.Serialization;
using LibraProgramming.ChatRoom.Common.Hessian.Extension;

namespace LibraProgramming.ChatRoom.Common.Hessian
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyElement : ISerializationElement
    {
        private string propertyname;
        private int? propertyOrder;

        /// <summary>
        /// 
        /// </summary>
        public Type ObjectType => Property.PropertyType;

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo Property
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ISerializationElement Element
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PropertyOrder
        {
            get
            {
                if (!propertyOrder.HasValue)
                {
                    var attribute = Property.GetCustomAttribute<DataMemberAttribute>();
                    propertyOrder = attribute?.Order ?? 0;
                }

                return propertyOrder.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get
            {
                if (String.IsNullOrEmpty(propertyname))
                {
                    propertyname = Property
                        .GetCustomAttribute<DataMemberAttribute>()
                        .Unless(attribute => String.IsNullOrEmpty(attribute.Name))
                        .Return(attribute => attribute.Name, Property.Name);
                }

                return propertyname;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="element"></param>
        public PropertyElement(PropertyInfo property, ISerializationElement element)
        {
            Property = property;
            Element = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="graph"></param>
        /// <param name="context"></param>
        public void Serialize(HessianOutputWriter writer, object graph, HessianSerializationContext context)
        {
            Element.Serialize(writer, graph, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Deserialize(HessianInputReader reader, HessianSerializationContext context)
        {
            return Element.Deserialize(reader, context);
        }
    }
}