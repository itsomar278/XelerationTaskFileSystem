using System.Linq.Expressions;

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


    }
}
