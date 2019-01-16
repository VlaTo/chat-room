namespace LibraProgramming.ChatRoom.Common.Hessian.Specification
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public class NotSpecification<TParam> : ISpecification<TParam>
    {
        private readonly ISpecification<TParam> specification;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        public NotSpecification(ISpecification<TParam> specification)
        {
            this.specification = specification;
        }

        /// <inheritdoc cref="ISpecification{TParam}.IsSatisfied" />
        public bool IsSatisfied(TParam arg)
        {
            return !specification.IsSatisfied(arg);
        }
    }
}