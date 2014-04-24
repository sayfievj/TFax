using System;

namespace TFax.Web.CORE.BLL.Infrastructure
{
    public class BusinessTransaction<T> : IDisposable where T : class 
    {
        
        private readonly IBaseRepository<T> _dbService;
         
        public BusinessTransaction(IBaseRepository<T> dbService)
        {
            _dbService = dbService;
            GetUnitOfWork().StartTransaction();
            IsInTransaction = true;
        }

        public bool IsInTransaction { get; set; }
 
        public void StartTransaction()
        {
            GetUnitOfWork().StartTransaction();
            IsInTransaction = true;
        }

        public void Complete()
        {
            GetUnitOfWork().Commit();
            IsInTransaction = false;
        }

        public void Dispose()
        {
            IsInTransaction = false;
            GetUnitOfWork().Dispose();
        }

        private IUnitOfWork GetUnitOfWork()
        {
            return _dbService.UnitOfWork;
        }

    }
}