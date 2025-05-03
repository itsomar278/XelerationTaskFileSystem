using XelerationTask.Core.Interfaces;

namespace XelerationTask.Infastructure.Persistence.UnitOfWorks
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly FileSystemDbContext _fileSystemDbContext;

        public IFolderRepository FolderRepository { get; }
        public IFileRepository FileRepository { get; }

        public IUserRepository UserRepository { get; }

        public UnitOfWork(FileSystemDbContext fileSystemDbContext, IFileRepository fileRepository,
            IFolderRepository folderRepository, IUserRepository userRepository)
        {
            _fileSystemDbContext = fileSystemDbContext;
            this.FileRepository = fileRepository;
            this.FolderRepository = folderRepository;
            this.UserRepository = userRepository;
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
