using System.Security.Claims;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFolderService
    {
        public Task<FolderResponseDTO> CreateFolder(FolderCreateDTO projectFolder, ClaimsPrincipal user);
        public Task<FolderResponseDTO> GetFolderAsync(int id);
        public Task DeleteFolderAsync(int id , ClaimsPrincipal user);
        public Task<ProjectFolder> GetByIdWithDetailsAsync(int id);
        public Task<bool> UpdateFolder(int id ,FolderUpdateDTO projectFolder, ClaimsPrincipal user);
        public Task<QueryResultDTO<FolderResponseDTO>> GetAllFolders(QueryParametersDTO parameters);
    }
}
