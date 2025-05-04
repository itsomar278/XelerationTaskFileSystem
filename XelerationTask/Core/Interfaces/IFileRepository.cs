using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFileRepository : IRepository<ProjectFile>
    {
        public Task<ProjectFile> GetByIdWithDetailsAsync(int id);
        public Task<QueryResult<ProjectFile>> GetAllAsyncMod(QueryParameters parameters);

    }
}
