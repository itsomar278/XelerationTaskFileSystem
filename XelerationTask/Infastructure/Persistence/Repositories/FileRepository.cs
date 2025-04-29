using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Infastructure.Persistence.Repositories
{
    public class FileRepository : Repository<ProjectFile> , IFileRepository
    {
        public FileRepository(DbContext dbContext) : base(dbContext) { }
    }
}
