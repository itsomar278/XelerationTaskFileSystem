using XelerationTask.Core.Exceptions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public  async Task<ProjectFile> CreateFile(ProjectFile projectFile)
        {
            var parentFolder = await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(projectFile.ParentFolderId);

            if (parentFolder==null) throw new ResourceNotFoundException("No Folder with this Id was found");

            var orderInFolder = await OrderInFolder(projectFile);

            if (orderInFolder > 0) projectFile.Name = projectFile.OriginalName + '(' + orderInFolder + ')';
            else projectFile.Name = projectFile.OriginalName;

            projectFile.CreatedAt = DateTime.Now;
            projectFile.UpdatedAt = DateTime.Now;

            await _unitOfWork.FileRepository.AddAsync(projectFile);
            await _unitOfWork.CompleteAsync();

            projectFile = await _unitOfWork.FileRepository.GetByIdWithDetailsAsync(projectFile.Id);

            return projectFile;

        }

        public async Task<ProjectFile> GetByIdWithDetailsAsync(int id)
        {
            var projectFile = await _unitOfWork.FileRepository.GetByIdWithDetailsAsync(id);
            if (projectFile == null) throw new ResourceNotFoundException("No File with this Id was found");
            return projectFile;
        }

        public async Task<QueryResult<ProjectFile>> GetAllFilesAsync(QueryParameters parameters)
        {
            var queryResult = await _unitOfWork.FileRepository.GetAllAsyncMod(parameters);
            if (queryResult == null || queryResult.Items.Count == 0) throw new ResourceNotFoundException("There is No Files yet");
            return queryResult;
        }

        public async Task DeleteFileAsync(int id)
        {
            var projectFile = await GetByIdWithDetailsAsync(id);
            projectFile.DeletedAt = DateTime.Now;
            _unitOfWork.FileRepository.SoftDelete(projectFile);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ProjectFile> UpdateFile(ProjectFile projectFile)
        {
            var existingFile = await GetByIdWithDetailsAsync(projectFile.Id);
            if (existingFile == null) throw new ResourceNotFoundException("No File with this Id was found");

            if (existingFile.ParentFolderId != projectFile.ParentFolderId)
            {
                var parentFolder = await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(projectFile.ParentFolderId);
                if (parentFolder == null) throw new ResourceNotFoundException("No Folder with this Id was found");
            }

            existingFile.OriginalName = projectFile.OriginalName;
            existingFile.ParentFolderId = projectFile.ParentFolderId;

            var orderInFolder = await OrderInFolder(projectFile);

            if (orderInFolder > 0) existingFile.Name = existingFile.OriginalName + '(' + orderInFolder + ')';
            else existingFile.Name = existingFile.OriginalName;

            existingFile.UpdatedAt = DateTime.Now;

            _unitOfWork.FileRepository.Update(existingFile);
            await _unitOfWork.CompleteAsync();
            return existingFile;
        }




        public async Task<int> OrderInFolder(ProjectFile projectFile)
        {

            var nameOccurences = await _unitOfWork.FileRepository.FindAsync(
                pf => pf.OriginalName.ToLower() == projectFile.OriginalName.ToLower() && pf.ParentFolderId == projectFile.ParentFolderId);

            if (nameOccurences == null) return 0;

            return nameOccurences.Count();
        }
    }
}
