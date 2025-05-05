using System.Linq.Expressions;
using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class

    {
        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        
        void SoftDelete(TEntity entity);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<QueryResult<TEntity>> GetPagedAsync(QueryParameters parameters);

    }
}
