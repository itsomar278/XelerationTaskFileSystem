using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FolderService: IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ProjectFolder> CreateFolder(ProjectFolder projectFolder)
        {
            if (projectFolder.ParentFolderId!=null && !await DoesParentExist(projectFolder)) throw new Exception("Parent Folder Not Found");
            if (await IsDuplicateInDirectory(projectFolder)) throw new Exception("A folder with the same name Exists in the same directory");

            projectFolder.CreatedAt = DateTime.Now;
            projectFolder.UpdatedAt = DateTime.Now;
            // created by later when adding user 
            // created by later when adding user 


            await _unitOfWork.FolderRepository.AddAsync(projectFolder);

            await _unitOfWork.CompleteAsync();

            projectFolder = await _unitOfWork.FolderRepository.GetAsync(projectFolder.Id);

            return projectFolder;

        }

        public async Task DeleteFolderAsync(int id)
        {
            var projectFolder = await GetByIdWithDetailsAsync(id);

            if (projectFolder == null) return;

            projectFolder.UpdatedAt = DateTime.Now;
            // deleted by later when adding user 
            // updated by later when adding user 
            _unitOfWork.FolderRepository.SoftDelete(projectFolder);

            foreach (var file in projectFolder.Files){
                
                _unitOfWork.FileRepository.SoftDelete(file);
            }

            foreach (var subFolder in projectFolder.SubFolders){

                await DeleteFolderAsync(subFolder.Id);
            }

            await _unitOfWork.CompleteAsync();

        }

        public async Task<ProjectFolder> GetByIdWithDetailsAsync(int id)
        {

           return await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(id);

        }

        public async Task<ProjectFolder> UpdateFolder(ProjectFolder projectFolder)
        {
            projectFolder.UpdatedAt = DateTime.Now;
            // updated by later 

            if (projectFolder.SubFolders.Any(sf => sf.Id == projectFolder.ParentFolderId)) throw new Exception("You can't create a circular dependency");


            _unitOfWork.FolderRepository.Update(projectFolder);

            await _unitOfWork.CompleteAsync();


            return projectFolder;
        }

        public async Task<bool> IsDuplicateInDirectory(ProjectFolder projectFolder) {

            var nameOccurences = await _unitOfWork.FolderRepository.FindAsync(
                pf => pf.Name.ToLower() == projectFolder.Name.ToLower() && !pf.IsDeleted && pf.ParentFolderId == projectFolder.ParentFolderId);

            if (nameOccurences==null || nameOccurences.Count() ==0) return false;

            return true;
        }

        public async Task<bool> DoesParentExist(ProjectFolder projectFolder)
        {
            if ( await _unitOfWork.FolderRepository.GetAsync((int)projectFolder.ParentFolderId) == null)
            {
                return false;
            }

            return true;
        }


        public FolderService(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;

        }

    }
}
