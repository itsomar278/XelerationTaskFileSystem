using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Infastructure.Persistence.Repositories
{
    public class FolderRepository : Repository<ProjectFolder> , IFolderRepository
    {
        public FolderRepository(FileSystemDbContext dbContext) : base(dbContext) { }

    }
}
