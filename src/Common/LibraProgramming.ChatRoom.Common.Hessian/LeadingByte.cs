using LibraProgramming.ChatRoom.Common.Hessian.Specification;

namespace LibraProgramming.ChatRoom.Common.Hessian
{
    /// <summary>
    /// 
    /// </summary>
    public class LeadingByte
    {
        /// <summary>
        /// 
        /// </summary>
        public byte Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNull => Check(If.Marker.Equals(Marker.Null));

        /// <summary>
        /// 
        /// </summary>
        public bool IsTrue => Check(If.Marker.Equals(Marker.True));

        /// <summary>
        /// 
        /// </summary>
        public bool IsFalse => Check(If.Marker.Equals(Marker.False));

        /// <summary>
        /// 
        /// </summary>
        public bool IsTinyInt32 => Check(If.Marker.Between(0x80).And(0xBF));

        /// <summary>
        /// 
        /// </summary>
        public bool IsShortInt32 => Check(If.Marker.Between(0xC0).And(0xCF));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactInt32 => Check(If.Marker.Between(0xD0).And(0xD7));

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnpackedInt32 => Check(If.Marker.Equals(Marker.UnpackedInteger));

        /// <summary>
        /// 
        /// </summary>
        public bool IsTinyInt64 => Check(If.Marker.Between(0xD8).And(0xEF));

        /// <summary>
        /// 
        /// </summary>
        public bool IsShortInt64 => Check(If.Marker.Between(0xF0).And(0xFF));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactInt64 => Check(If.Marker.Between(0x38).And(0x3F));

        /// <summary>
        /// 
        /// </summary>
        public bool IsPackedInt64 => Check(If.Marker.Equals(Marker.PackedLong));

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnpackedInt64 => Check(If.Marker.Equals(Marker.UnpackedLong));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactBinary => Check(If.Marker.Between(0x20).And(0x2f));

        /// <summary>
        /// 
        /// </summary>
        public bool IsNonfinalChunkBinary => Check(If.Marker.Equals(Marker.BinaryNonfinalChunk));

        /// <summary>
        /// 
        /// </summary>
        public bool IsFinalChunkBinary => Check(If.Marker.Equals(Marker.BinaryFinalChunk));

        /// <summary>
        /// 
        /// </summary>
        public bool IsZeroDouble => Check(If.Marker.Equals(Marker.DoubleZero));

        /// <summary>
        /// 
        /// </summary>
        public bool IsOneDouble => Check(If.Marker.Equals(Marker.DoubleOne));

        /// <summary>
        /// 
        /// </summary>
        public bool IsTinyDouble => Check(If.Marker.Equals(Marker.DoubleOctet));

        /// <summary>
        /// 
        /// </summary>
        public bool IsShortDouble => Check(If.Marker.Equals(Marker.DoubleShort));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactDouble => Check(If.Marker.Equals(Marker.DoubleFloat));

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnpackedDouble => Check(If.Marker.Equals(Marker.Double));

        /// <summary>
        /// 
        /// </summary>
        public bool IsTinyString => Check(If.Marker.Between(0x00).And(0x1F));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactString => Check(If.Marker.Between(0x30).And(0x33));

        /// <summary>
        /// 
        /// </summary>
        public bool IsNonfinalChunkString => Check(If.Marker.Equals(Marker.StringNonfinalChunk));

        /// <summary>
        /// 
        /// </summary>
        public bool IsFinalChunkString => Check(If.Marker.Equals(Marker.StringFinalChunk));

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompactDateTime => Check(If.Marker.Equals(Marker.DateTimeCompact));

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnpackedDateTime => Check(If.Marker.Equals(Marker.DateTimeLong));

        /// <summary>
        /// 
        /// </summary>
        public bool IsClassDefinition => Check(If.Marker.Equals(Marker.ClassDefinition));

        /// <summary>
        /// 
        /// </summary>
        public bool IsShortObjectReference => Check(If.Marker.Between(0x60).And(0x6F));

        /// <summary>
        /// 
        /// </summary>
        public bool IsLongObjectReference => Check(If.Marker.Equals(Marker.ClassReference));

        /// <summary>
        /// 
        /// </summary>
        public bool IsInstanceReference => Check(If.Marker.Equals(Marker.InstanceReference));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetData(byte value)
        {
            Data = value;
        }

        private bool Check(ISpecification<byte> specification)
        {
            return specification.IsSatisfied(Data);
        }

        /// <summary>
        /// 
        /// </summary>
        private static class If
        {
            /// <summary>
            /// 
            /// </summary>
            internal static class Marker
            {
                public static MarkerValue Equals(byte value)
                {
                    return new MarkerValue(value);
                }

                public static MarkerMinValue Between(byte value)
                {
                    return new MarkerMinValue(value);
                }

                /// <summary>
                /// 
                /// </summary>
                internal abstract class MarkerSpecification : ISpecification<byte>
                {
                    public abstract bool IsSatisfied(byte arg);
                }

                /// <summary>
                /// 
                /// </summary>
                internal class MarkerValue : MarkerSpecification
                {
                    protected readonly byte value;

                    public MarkerValue(byte value)
                    {
                        this.value = value;
                    }

                    public override bool IsSatisfied(byte arg)
                    {
                        return value == arg;
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                internal class MarkerMinValue : MarkerValue
                {
                    public MarkerMinValue(byte value)
                        : base(value)
                    {
                    }

                    public override bool IsSatisfied(byte arg)
                    {
                        return value <= arg;
                    }

                    public ISpecification<byte> And(byte max)
                    {
                        return this.And(new MarkerMaxValue(max));
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                private class MarkerMaxValue : MarkerValue
                {
                    public MarkerMaxValue(byte value)
                        : base(value)
                    {
                    }

                    public override bool IsSatisfied(byte arg)
                    {
                        return value >= arg;
                    }
                }
            }
        }
    }
}