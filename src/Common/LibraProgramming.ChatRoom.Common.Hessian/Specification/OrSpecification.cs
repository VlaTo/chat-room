namespace LibraProgramming.ChatRoom.Common.Hessian.Specification
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public class OrSpecification<TParam> : BinarySpecification<TParam>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public OrSpecification(ISpecification<TParam> left, ISpecification<TParam> right)
            : base(left, right)
        {
        }

        /// <inheritdoc cref="BinarySpecification{TParam}.IsSatisfied" />
        public override bool IsSatisfied(TParam arg)
        {
            return Left.IsSatisfied(arg) || Right.IsSatisfied(arg);
        }
    }
}