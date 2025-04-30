using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Interfaces;

namespace XelerationTask.Infastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly FileSystemDbContext _DbContext;

        public Repository(FileSystemDbContext context)
        {
            _DbContext = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _DbContext.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _DbContext.Set<TEntity>().Remove(entity);
            return;
        }

        public void Update(TEntity entity)
        {
            _DbContext.Set<TEntity>().Update(entity);
            return;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }
    }
}

