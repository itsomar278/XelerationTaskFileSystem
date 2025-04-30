using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFolderRepository : IRepository<ProjectFolder>
    {
        public Task<ProjectFolder?> GetByIdWithDetailsAsync(int id);
    }
}
