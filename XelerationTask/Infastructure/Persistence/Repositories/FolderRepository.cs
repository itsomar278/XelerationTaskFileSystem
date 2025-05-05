using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using XelerationTask.Core.Extensions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Infastructure.Persistence.Repositories
{
    public class FolderRepository : Repository<ProjectFolder> , IFolderRepository
    {
        public FolderRepository(FileSystemDbContext dbContext) : base(dbContext) { }

        public async Task<ProjectFolder?> GetByIdWithDetailsAsync(int id)
        {
            return await _DbContext.ProjectFolders
                .Include(f => f.ParentFolder)
                .Include(f => f.SubFolders)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<QueryResult<ProjectFolder>> GetAllAsyncMod(QueryParameters parameters)
        {
            var baseQuery = _DbContext.ProjectFolders
                .Include(f => f.ParentFolder)
                .Include(f => f.SubFolders)
                .Include(f => f.Files)
                .AsQueryable();

            return await baseQuery.ToQueryResultAsync(parameters);
        }


    }
}
