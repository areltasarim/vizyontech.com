using EticaretWebCoreEntity.Infrastructure;
using System.Transactions;

namespace EticaretWebCoreEntity
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;

        public UnitOfWork()
        {
            _context = new AppDbContext();
        }
        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_context);
        }


        public async Task CompleteAsync()
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.SaveChangesAsync();
                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                hata.Message.ToString();
                throw;
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
