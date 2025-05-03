namespace XelerationTask.Core.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IFolderRepository FolderRepository { get; }

        public IFileRepository FileRepository { get; }

        public IUserRepository UserRepository { get; }

        public Task<int> CompleteAsync();

    }
}
