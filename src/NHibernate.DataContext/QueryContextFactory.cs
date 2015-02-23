namespace NHibernate.DataContext
{
    using NHibernate;
    using NHibernate.Linq;

    static class QueryContextFactory
    {
        public static DbRepoQueryContext<T> CreateDbRepoQueryContext<T>(ISession session, bool isReadonly) where T : class
        {
            return new DbRepoQueryContext<T>(session, session.Query<T>().Expression, isReadonly);
        }
    }
}
