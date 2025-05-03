using Microsoft.AspNetCore.Mvc;
using XelerationTask.Core.Exceptions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FolderService: IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ProjectFolder> CreateFolder(ProjectFolder projectFolder)
        {
            if (projectFolder.ParentFolderId!=null && !await DoesParentExist(projectFolder)) throw new ResourceNotFoundException($"Parent Folder with id : {projectFolder.Id} Not Found");

            if (await IsDuplicateInDirectory(projectFolder)) throw new ResourceAlreadyExistsException($"A folder with the same name : {projectFolder.Name} Exists in the same directory"); 

            projectFolder.CreatedAt = DateTime.Now;
            projectFolder.UpdatedAt = DateTime.Now;
            // created by later when adding user 
            // created by later when adding user 


            await _unitOfWork.FolderRepository.AddAsync(projectFolder);

            await _unitOfWork.CompleteAsync();

            projectFolder = await _unitOfWork.FolderRepository.GetAsync(projectFolder.Id);

            return projectFolder;

        }

        public async Task<QueryResult<ProjectFolder>> GetAllFolders(QueryParameters parameters)
        {
            var queryResult = await _unitOfWork.FolderRepository.GetAllAsyncMod(parameters);

            if (queryResult == null || queryResult.Items.Count == 0) throw new ResourceNotFoundException("There is No Folders yet");

            return queryResult;
        }

        public async Task DeleteFolderAsync(int id)
        {
            var projectFolder = await GetByIdWithDetailsAsync(id);

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
            var projectFolder = await _unitOfWork.FolderRepository.GetAsync(id);

            if(projectFolder == null) throw new ResourceNotFoundException($"Project Folder with {id} Not Found");

            return projectFolder;
        }

        public async Task<ProjectFolder> UpdateFolder(ProjectFolder projectFolder)
        {
            projectFolder.UpdatedAt = DateTime.Now;
            // updated by later 

            if (projectFolder.SubFolders.Any(sf => sf.Id == projectFolder.ParentFolderId) ||
                projectFolder.ParentFolderId == projectFolder.Id)
                throw new InvalidOperationError("A folder can't be the child of itself or one of his childs ");

            if (projectFolder.ParentFolderId != null && !await DoesParentExist(projectFolder)) throw new ResourceNotFoundException($"Parent Folder with id : {projectFolder.ParentFolderId} Not Found");

            if (await IsDuplicateInDirectory(projectFolder)) throw new ResourceAlreadyExistsException($"A folder with the same name : {projectFolder.Name} Exists in the same directory");

            _unitOfWork.FolderRepository.Update(projectFolder);

            await _unitOfWork.CompleteAsync();


            return projectFolder;
        }

        public async Task<bool> IsDuplicateInDirectory(ProjectFolder projectFolder) {

            var nameOccurences = await _unitOfWork.FolderRepository.FindAsync(
                pf => pf.Name.ToLower() == projectFolder.Name.ToLower()  && pf.ParentFolderId == projectFolder.ParentFolderId);

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
