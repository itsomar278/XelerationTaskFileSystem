using System.Security.Claims;
using AutoMapper;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Exceptions;
using XelerationTask.Core.Extensions;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FileService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }


        public async Task<FileResponseDTO> CreateFileAsync(FileCreateDTO fileCreateDTO, ClaimsPrincipal user)
        {
            var projectFile = _mapper.Map<ProjectFile>(fileCreateDTO);

            var parentFolder = await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(projectFile.ParentFolderId);

            if (parentFolder == null)
                throw new ResourceNotFoundException("No Folder with this Id was found");

            projectFile.Name = await GenerateFileNameWithOrder(projectFile, projectFile);

            projectFile.CreatedAt = DateTime.Now;
            projectFile.UpdatedAt = DateTime.Now;
            projectFile.DeletedAt = null;
            projectFile.CreatedBy = user.GetUserId();
            projectFile.UpdatedBy = user.GetUserId();

            await _unitOfWork.FileRepository.AddAsync(projectFile);
            await _unitOfWork.CompleteAsync();

            var fileResponse = _mapper.Map<FileResponseDTO>(projectFile);

            return fileResponse;
        }

        public async Task<FileResponseDTO> GetFileAsync(int id)
        {
            var projectFile = await GetByIdWithDetailsAsync(id);
            var fileResponse = _mapper.Map<FileResponseDTO>(projectFile);
            return fileResponse;
        }

        public async Task<ProjectFile> GetByIdWithDetailsAsync(int id)
        {
            var projectFile = await _unitOfWork.FileRepository.GetByIdWithDetailsAsync(id);
            if (projectFile == null) throw new ResourceNotFoundException("No File with this Id was found");
            return projectFile;
        }

        public async Task<QueryResultDTO<FileResponseDTO>> GetAllFilesAsync(QueryParametersDTO queryParametersDTO)
        {
            var parameters = _mapper.Map<QueryParameters>(queryParametersDTO);
            var queryResult = await _unitOfWork.FileRepository.GetPagedAsync(parameters);
            if (queryResult == null || queryResult.Items.Count() == 0) throw new ResourceNotFoundException("There is No Files yet");
            var filesToReturn = _mapper.Map<QueryResultDTO<FileResponseDTO>>(queryResult);
            return filesToReturn;
        }

        public async Task DeleteFileAsync(int id , ClaimsPrincipal user)
        {
            var projectFile = await GetByIdWithDetailsAsync(id);
            projectFile.DeletedAt = DateTime.Now;
            projectFile.DeletedBy = user.GetUserId();
            _unitOfWork.FileRepository.SoftDelete(projectFile);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> UpdateFileAsync(int id, FileUpdateDTO fileUpdateDTO , ClaimsPrincipal user)
        {
            var existingFile = await GetByIdWithDetailsAsync(id);

            var projectFile = _mapper.Map(fileUpdateDTO, existingFile);

            if (projectFile.ParentFolderId != existingFile.ParentFolderId)
            {
                var parentFolder = await _unitOfWork.FolderRepository.GetByIdWithDetailsAsync(projectFile.ParentFolderId);
                if (parentFolder == null) throw new ResourceNotFoundException("No Folder with this Id was found");
            }
            existingFile.OriginalName = projectFile.OriginalName;
            existingFile.ParentFolderId = projectFile.ParentFolderId;

            existingFile.Name = await GenerateFileNameWithOrder(existingFile, projectFile);

            existingFile.UpdatedAt = DateTime.Now;
            existingFile.UpdatedBy = user.GetUserId();

            _unitOfWork.FileRepository.Update(existingFile);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        private async Task<string> GenerateFileNameWithOrder(ProjectFile existingFile, ProjectFile projectFile)
        {
            var orderInFolder = await OrderInFolder(projectFile);

            return orderInFolder > 0
                ? existingFile.OriginalName + '(' + orderInFolder + ')'
                : existingFile.OriginalName;
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
