using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FolderService: IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ProjectFolder> CreateFolder(ProjectFolder projectFolder)
        {

            if (await IsDuplicateInDirectory(projectFolder)) throw new Exception(); // Resource Exists 403 custome exception later 

            await _unitOfWork.FolderRepository.AddAsync(projectFolder);

            await _unitOfWork.CompleteAsync();

            projectFolder = await _unitOfWork.FolderRepository.GetAsync(projectFolder.Id);

             // add extra mapping her to return folder response 
            return projectFolder;

        }

        public async Task<ProjectFolder> GetByIdWithDetailsAsync(int id)
        {

           return await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(id);

        }

        public async Task<bool> IsDuplicateInDirectory(ProjectFolder projectFolder) {

            var nameOccurences = await _unitOfWork.FolderRepository.FindAsync(
                pf => pf.Name.ToLower() == projectFolder.Name.ToLower() && !pf.IsDeleted && pf.ParentFolderId == projectFolder.ParentFolderId);

            if (nameOccurences==null || nameOccurences.Count() ==0) return false;

            return true;
        }


        public FolderService(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;

        }

    }
}
