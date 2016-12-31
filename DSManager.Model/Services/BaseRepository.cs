using System;
using System.Linq;

using NHibernate;
using NHibernate.Linq;

using DSManager.Model.Services.Interfaces;

namespace DSManager.Model.Services {
    public class BaseRepository : IRepository, IDisposable {
        protected ISession Session;
        protected ITransaction Transaction;

        public BaseRepository() {
            Session = NHibernateConfiguration.OpenSession();
        }
        public BaseRepository(ISession session) {
            Session = session;
        }

        #region Transaction and Session Management Methods
        public void BeginTransaction() {
            Transaction = Session.BeginTransaction();
        }
        public void CommitTransaction() {
            // _transaction will be replaced with a new transaction by NHibernate, but we will close to keep a consistent state.
            Transaction.Commit();
            CloseTransaction();
        }
        public void RollbackTransaction() {
            // _session must be closed and disposed after a transaction rollback to keep a consistent state.
            Transaction.Rollback();
            CloseTransaction();
            CloseSession();
        }
        private void CloseTransaction() {
            Transaction.Dispose();
            Transaction = null;
        }
        private void CloseSession() {
            Session.Close();
            Session.Dispose();
            Session = null;
        }
        #endregion

        #region IRepository Members
        public virtual void Save(object obj) {
            Session.SaveOrUpdate(obj);
        }
        public virtual void Delete(object obj) {
            Session.Delete(obj);
        }
        public virtual object GetById(Type objType, object objId) {
            return Session.Load(objType, objId);
        }
        public virtual IQueryable<TEntity> ToList<TEntity>() {
            return (from entity in Session.Query<TEntity>() select entity);
        }
        #endregion

        #region IDisposable Members
        public void Dispose() {
            if(Transaction != null) {
                // Commit transaction by default, unless user explicitly rolls it back.
                // To rollback transaction by default, unless user explicitly commits, comment out the line below.
                CommitTransaction();
            }
            if(Session != null) {
                Session.Flush(); // commit session transactions
                CloseSession();
            }
        }
        #endregion
    }
}
