namespace EticaretWebCoreEntity.Infrastructure
{
    public interface IUnitOfWork
    {

        Task CompleteAsync();

        void Dispose();
    }
}