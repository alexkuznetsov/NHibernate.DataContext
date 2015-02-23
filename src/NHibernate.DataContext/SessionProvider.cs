namespace NHibernate.DataContext
{
    using NHibernate;
    using System;
    using System.Configuration;

    static class SessionProvider
    {
        private static string _connectionString;

        private static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["SampleDomain"].ConnectionString;
                }

                return _connectionString;
            }
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                throw new NotImplementedException("Put here your implementation of session bulder!");
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }
    }
}
