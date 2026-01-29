using System.Linq.Expressions;

namespace EticaretWebCoreEntity.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);

        Task Add(IEnumerable<T> items);

        void Update(T entity);

        void Update(IEnumerable<T> items);

        Task<T> Delete(int id);
        Task Delete(IEnumerable<T> entities);

        Task<IQueryable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter);

        //public async Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression)
        //{
        //    using (var dbContext = new AppDbContext())
        //    {
        //        var query = dbContext.Set<T>().Where(expression);
        //        return query; // await Task.FromResult(query); bu satırı kaldırın
        //    }
        //}
    }
}
