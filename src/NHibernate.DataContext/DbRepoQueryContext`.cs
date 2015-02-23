namespace NHibernate.DataContext
{
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class DbRepoQueryContext<TEntity> : DbQueryContext<TEntity> where TEntity : class
    {
        private bool m_isReadOnly;

        public DbRepoQueryContext(ISession session, Expression expression, bool isReadOnly)
            : base(session, expression)
        {
            m_isReadOnly = isReadOnly;
        }

        public TEntity Find(object id)
        {
            return Session.Get<TEntity>(id);
        }

        public TEntity Fetch(object id)
        {
            return Find(id);
        }

        private void TestIsReadonly()
        {
            if (m_isReadOnly)
            {
                throw new InvalidOperationException("Collection is readonly!");
            }
        }

        public TEntity Add(TEntity entity)
        {
            TestIsReadonly();

            ITransaction trans = null;
            try
            {
                trans = Session.BeginTransaction();

                entity = AddInternal(entity);

                trans.Commit();
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }

            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> range)
        {
            TestIsReadonly();

            ITransaction trans = null;
            try
            {
                trans = Session.BeginTransaction();

                var resp = new List<TEntity>();

                foreach (var e in range)
                {
                    resp.Add(AddInternal(e));
                }

                trans.Commit();

                return resp;
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        public void Delete(TEntity entity)
        {
            TestIsReadonly();

            ITransaction trans = null;

            try
            {
                trans = Session.BeginTransaction();
                DeleteInternal(entity);
                trans.Commit();
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        public void DeleteRange(IEnumerable<TEntity> range)
        {
            TestIsReadonly();

            ITransaction trans = null;
            try
            {
                trans = Session.BeginTransaction();

                foreach (var i in range)
                    DeleteInternal(i);

                trans.Commit();
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        public TEntity Update(TEntity entity)
        {
            return Update(entity, "U");
        }

        public TEntity Update(TEntity entity, string corrType)
        {
            TestIsReadonly();

            ITransaction trans = null;
            try
            {
                trans = Session.BeginTransaction();

                entity = UpdateInternal(entity, corrType);

                trans.Commit();

                return entity;
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        public List<TEntity> UpdateRange(IEnumerable<TEntity> range)
        {
            return UpdateRange(range, "U");
        }

        public List<TEntity> UpdateRange(IEnumerable<TEntity> range, string corrType)
        {
            TestIsReadonly();

            var returnVal = new List<TEntity>();
            ITransaction trans = null;
            try
            {
                trans = Session.BeginTransaction();

                foreach (var i in range)
                    returnVal.Add(UpdateInternal(i, corrType));

                trans.Commit();

                return returnVal;
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw;
            }

            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
        }

        #region Internals

        private TEntity AddInternal(TEntity entity)
        {
            TestIsReadonly();

            entity.SetProperty("CorrType", "I");

            if (!entity.HasProperty("DateBegin"))
            {
                entity.SetProperty("DateBegin", DateTime.Now);
            }

            return Session.Merge<TEntity>(entity);
        }

        private TEntity UpdateInternal(TEntity entity)
        {
            return UpdateInternal(entity, "U");
        }

        private TEntity UpdateInternal(TEntity entity, string corrType)
        {
            TestIsReadonly();

            entity.SetProperty("CorrType", corrType);

            if (!entity.HasProperty("DateBegin"))
            {
                entity.SetProperty("DateBegin", DateTime.Now);
            }



            return Session.Merge(entity);
        }

        private void DeleteInternal(TEntity entity)
        {
            TestIsReadonly();

            var idVal = entity.GetProperty("Id");
            var e = Session.Get<TEntity>(idVal);

            if (e != null)
            {
                e.SetProperty("CorrType", "D");
                e.SetProperty("DateEnd", DateTime.Now);

                Session.Update(e);

                return;
            }

            throw new ArgumentOutOfRangeException("Entity not found!");
        }

        #endregion
    }
}
