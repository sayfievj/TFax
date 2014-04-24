using System.Data.Entity;
using System.Transactions;
using System.Web.Mvc;

using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.BLL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        private readonly TFaxEntities _db;

        public UnitOfWork()
        {
            _db = (TFaxEntities)DependencyResolver.Current.GetService<DbContext>();
        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _db.SaveChanges();
            _transaction.Complete();
        }

        public DbContext Db
        {
            get { return _db; }
        }

        public void Dispose()
        {
        }

    }
}
