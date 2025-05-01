using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFolderService
    {
        public Task<ProjectFolder> CreateFolder(ProjectFolder projectFolder);
        public Task<ProjectFolder> GetByIdWithDetailsAsync(int id);
        public Task DeleteFolderAsync(int id);
        public Task<ProjectFolder> UpdateFolder(ProjectFolder projectFolder);
    }
}
