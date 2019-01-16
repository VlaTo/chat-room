namespace LibraProgramming.ChatRoom.Common.Hessian.Specification
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public abstract class BinarySpecification<TParam> : ISpecification<TParam>
    {
        /// <summary>
        /// 
        /// </summary>
        public ISpecification<TParam> Left
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ISpecification<TParam> Right
        {
            get;
            protected set;
        }

        protected BinarySpecification(ISpecification<TParam> left, ISpecification<TParam> right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public abstract bool IsSatisfied(TParam arg);
    }
}