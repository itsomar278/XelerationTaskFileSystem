using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Exceptions;
using XelerationTask.Core.Extensions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FolderService: IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public async Task<FolderResponseDTO> CreateFolder(FolderCreateDTO folderCreateDTO, ClaimsPrincipal user)
        {
            var projectFolder = _mapper.Map<ProjectFolder>(folderCreateDTO);

            if (projectFolder.ParentFolderId!=null && !await DoesParentExist(projectFolder)) throw new ResourceNotFoundException($"Parent Folder with id : {projectFolder.Id} Not Found");

            if (await IsDuplicateInDirectory(projectFolder)) throw new ResourceAlreadyExistsException($"A folder with the same name : {projectFolder.Name} Exists in the same directory"); 

            projectFolder.CreatedAt = DateTime.Now;
            projectFolder.UpdatedAt = DateTime.Now;
            projectFolder.CreatedBy = user.GetUserId();
            projectFolder.UpdatedBy = user.GetUserId();


            await _unitOfWork.FolderRepository.AddAsync(projectFolder);

            await _unitOfWork.CompleteAsync();


            var response = _mapper.Map<FolderResponseDTO>(projectFolder);

            return response;

        }
        public async Task<FolderResponseDTO> GetFolderAsync(int id)
        {
            var folder = await GetByIdWithDetailsAsync(id);

            var response = _mapper.Map<FolderResponseDTO>(folder);

            return response;
        }


        public async Task<QueryResultDTO<FolderResponseDTO>> GetAllFolders(QueryParametersDTO queryParametersDTO)
        {
            var parameters = _mapper.Map<QueryParameters>(queryParametersDTO);

            var queryResult = await _unitOfWork.FolderRepository.GetPagedAsync(parameters);

            if (queryResult == null || queryResult.Items.Count() == 0) throw new ResourceNotFoundException("There is No Folders yet");

            var dtoResult = _mapper.Map<QueryResultDTO<FolderResponseDTO>>(queryResult);

            return dtoResult;
        }

        public async Task DeleteFolderAsync(int id, ClaimsPrincipal user)
        {
            var projectFolder = await GetByIdWithDetailsAsync(id);

            projectFolder.UpdatedAt = DateTime.Now;
            projectFolder.DeletedAt = DateTime.Now;
            projectFolder.UpdatedBy = user.GetUserId();
            projectFolder.DeletedBy = user.GetUserId();

            _unitOfWork.FolderRepository.SoftDelete(projectFolder);

            foreach (var file in projectFolder.Files){

                file.DeletedBy = user.GetUserId();
                _unitOfWork.FileRepository.SoftDelete(file);
            }

            foreach (var subFolder in projectFolder.SubFolders){

                await DeleteFolderAsync(subFolder.Id , user);
            }

            await _unitOfWork.CompleteAsync();

        }

        public async Task<ProjectFolder> GetByIdWithDetailsAsync(int id)
        {
            var projectFolder = await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(id); 

            if(projectFolder == null) throw new ResourceNotFoundException($"Project Folder with {id} Not Found");

            return projectFolder;
        }

        public async Task<bool> UpdateFolder(int id ,FolderUpdateDTO folderUpdateDTO , ClaimsPrincipal user)
        {
            var existingFolder = await GetByIdWithDetailsAsync(id);

            var projectFolder = _mapper.Map(folderUpdateDTO, existingFolder);

            projectFolder.UpdatedAt = DateTime.Now;
            projectFolder.UpdatedBy = user.GetUserId();
 

            if (projectFolder.SubFolders.Any(sf => sf.Id == projectFolder.ParentFolderId) ||
                projectFolder.ParentFolderId == projectFolder.Id)
                throw new InvalidOperationError("A folder can't be the child of itself or one of his childs ");

            if (projectFolder.ParentFolderId != null && !await DoesParentExist(projectFolder)) throw new ResourceNotFoundException($"Parent Folder with id : {projectFolder.ParentFolderId} Not Found");

            if (await IsDuplicateInDirectory(projectFolder)) throw new ResourceAlreadyExistsException($"A folder with the same name : {projectFolder.Name} Exists in the same directory");

            _unitOfWork.FolderRepository.Update(projectFolder); 

            await _unitOfWork.CompleteAsync();

            return true;
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


        public FolderService(IUnitOfWork unitOfWork , IMapper mapper) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

    }
}
