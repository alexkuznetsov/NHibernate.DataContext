namespace NHibernate.DataContext
{
    using NHibernate;
    using System.Linq.Expressions;

    public class DbQueryContext<TEntity> : Queryable<TEntity>
        where TEntity : class
    {
        private ISession m_session;

        protected ISession Session
        {
            get { return m_session; }
        }

        public DbQueryContext(ISession session, Expression expression)
            : base(new NHibernate.Linq.DefaultQueryProvider(session.GetSessionImplementation()), expression)
        {
            m_session = session;
        }
    }
}
