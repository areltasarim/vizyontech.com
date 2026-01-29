using EticaretWebCoreEntity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EticaretWebCoreEntity
{
    public class Repository<T> : IRepository<T> where T : class
    {

        DbSet<T> Table { get; set; }

        public Repository(AppDbContext context)
        {
            Table = context.Set<T>();
        }

        public async Task Add(T entity) => await Table.AddAsync(entity);

        public async Task Add(IEnumerable<T> items) => await Table.AddRangeAsync(items);

        public Task<IQueryable<T>> GetAll() => Task.FromResult(Table.AsQueryable<T>());
        public async Task<T> Delete(int id)
        {
            var entity = await Table.FindAsync(id);
            Table.Remove(entity);
            return entity;
        }

      
        public async Task Delete(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);
        }

        public async Task<T> GetById(int id) => await new AppDbContext().Set<T>().FindAsync(id);
        public async Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await new AppDbContext().Set<T>().AsNoTracking().SingleOrDefaultAsync(filter);
        }

        public void Update(T entity) => Table.Update(entity);

        public void Update(IEnumerable<T> items) => Table.UpdateRange(items);


        public IQueryable<T> Where(Expression<Func<T, bool>> expression) => Table.Where(expression);

    }
}