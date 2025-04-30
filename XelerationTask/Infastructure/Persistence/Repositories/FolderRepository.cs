using Microsoft.EntityFrameworkCore;
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

    }
}
