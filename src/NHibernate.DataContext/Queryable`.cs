namespace NHibernate.DataContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class Queryable<T> : IOrderedQueryable<T>
    {
        internal Queryable(IQueryProvider provider, Expression expression)
        {
            Initialize(provider, expression);
        }

        private void Initialize(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (expression != null && !typeof(IQueryable<T>).
                   IsAssignableFrom(expression.Type))
                throw new ArgumentException(
                     String.Format("Not assignable from {0}", expression.Type), "expression");

            Provider = provider;
            var expressionFix = (IOrderedQueryable<T>) this;
            Expression = expression ?? Expression.Constant(expressionFix);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}
