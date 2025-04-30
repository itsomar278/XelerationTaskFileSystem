using XelerationTask.Core.Interfaces;

namespace XelerationTask.Infastructure.Persistence.IUnitOfWorks
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly FileSystemDbContext _fileSystemDbContext;

        public IFolderRepository FolderRepository { get; }
        public IFileRepository FileRepository { get; }

        public UnitOfWork(FileSystemDbContext fileSystemDbContext, IFileRepository fileRepository, IFolderRepository folderRepository)
        {
            _fileSystemDbContext = fileSystemDbContext;
            this.FileRepository = fileRepository;
            this.FolderRepository = folderRepository;
        }

        public async Task<int> CompleteAsync()
        {
           return await _fileSystemDbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _fileSystemDbContext.DisposeAsync();
        }
    }
}
