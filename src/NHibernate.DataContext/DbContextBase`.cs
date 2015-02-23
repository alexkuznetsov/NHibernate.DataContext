namespace NHibernate.DataContext
{
    using NHibernate;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DbContextBase<TContext> : IDisposable
        where TContext : class
    {
        private bool m_disposed;
        private readonly ISession m_session;

        private static Dictionary<Type, PropertyInfo> m_propertiesCache = null;
        private static readonly object m_loker = new object();

        public DbContextBase()
        {
            m_session = SessionProvider.OpenSession();
        }

        ~DbContextBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!m_disposed)
                {
                    m_session.Dispose();
                    m_disposed = true;
                }
            }
        }

        protected ISession Session { get { return m_session; } }

        public IList<T> ExecuteSqlWithResult<T>(string sql, object parameters = null)
        {
            IDictionary<string, object> dic = null;
            if (parameters != null)
            {
                dic = parameters.ToDictionary();
            }

            IList<T> rs = null;

            var query = Session.CreateSQLQuery(sql);

            if (dic != null)
            {
                foreach (var pData in dic)
                {
                    query.SetParameter(pData.Key, pData.Value);
                }
            }

            rs = query.AddEntity(typeof(T)).List<T>();

            return rs;
        }

        public object ExecuteUndefined(string sql, object parameters = null)
        {
            IDictionary<string, object> dic = null;
            if (parameters != null)
            {
                dic = parameters.ToDictionary();
            }

            bool rs = false;
            object[] data;

            var query = Session.CreateSQLQuery(sql);

            if (dic != null)
            {
                foreach (var pData in dic)
                {
                    query.SetParameter(pData.Key, pData.Value);
                }
            }

            data = query.List<object>().ToArray();

            if (data == null || data.Length == 0 || !bool.TryParse((data[0] ?? "").ToString(), out rs))
            {
                return false;
            }
            else
            {
                return rs;
            }
        }

        public object ExecuteWithRecords(string sql, object parameters = null)
        {
            IDictionary<string, object> dic = null;
            if (parameters != null)
            {
                dic = parameters.ToDictionary();
            }

            var query = Session.CreateSQLQuery(sql);

            if (dic != null)
            {
                foreach (var pData in dic)
                {
                    query.SetParameter(pData.Key, pData.Value);
                }
            }

            ArrayList data = (ArrayList)query.List();

            return data;
        }

        protected DbRepoQueryContext<TEnitity> CreateRepoInternalSet<TEnitity>(bool isReadonly) where TEnitity : class
        {
            return QueryContextFactory.CreateDbRepoQueryContext<TEnitity>(Session, isReadonly);
        }

        public DbRepoQueryContext<TEntity> Set<TEntity>()
            where TEntity : class
        {
            if (m_propertiesCache == null)
            {
                lock (m_loker)
                {
                    m_propertiesCache = GetType().GetProperties()
                        .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbRepoQueryContext<>))
                        .ToDictionary(x => x.PropertyType.GetGenericArguments().First(), x => x);
                }
            }

            if (m_propertiesCache.ContainsKey(typeof(TEntity)))
            {
                var prop = m_propertiesCache[typeof(TEntity)];

                return prop.GetValue(this) as DbRepoQueryContext<TEntity>;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
