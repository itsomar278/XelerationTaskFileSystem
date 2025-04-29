namespace XelerationTask.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class

    {
        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

    }
}
