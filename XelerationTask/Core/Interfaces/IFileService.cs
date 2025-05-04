using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFileService
    {
        public Task<ProjectFile> CreateFile(ProjectFile projectFile);
        public Task<ProjectFile> GetByIdWithDetailsAsync(int id);
        public Task DeleteFileAsync(int id);
        public Task<ProjectFile> UpdateFile(ProjectFile ProjectFile);
        public Task<QueryResult<ProjectFile>> GetAllFilesAsync(QueryParameters parameters);

    }
}
