using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Extensions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Infastructure.Persistence.Repositories
{
    public class FileRepository : Repository<ProjectFile>, IFileRepository
    {
        public FileRepository(FileSystemDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ProjectFile> GetByIdWithDetailsAsync(int id)
        {
            var projectFile = await _DbContext.ProjectFiles
                .Include(pf => pf.ParentFolder)
                .FirstOrDefaultAsync(pf => pf.Id == id);
            return projectFile;
        }

        public async Task<QueryResult<ProjectFile>> GetAllAsyncMod(QueryParameters parameters)
        {
            var baseQuery = _DbContext.ProjectFiles
                .Include(f => f.ParentFolder)
                .AsQueryable();

            var filteredSortedQuery = baseQuery
                .ApplyFiltering(parameters)
                .ApplySorting(parameters);

            var totalCount = await filteredSortedQuery.CountAsync();

            var pagedResult = await filteredSortedQuery
                .ApplyPagination(parameters)
                .ToListAsync();

            return new QueryResult<ProjectFile>
            {
                Items = pagedResult,
                Page = parameters.Page,
                PageSize = parameters.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
