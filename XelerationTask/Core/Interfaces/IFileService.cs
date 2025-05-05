using System.Security.Claims;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IFileService
    {
        public Task<FileResponseDTO> CreateFileAsync(FileCreateDTO fileCreateDTO, ClaimsPrincipal user);
        protected Task<ProjectFile> GetByIdWithDetailsAsync(int id);
        public Task DeleteFileAsync(int id, ClaimsPrincipal user);
        public Task<bool> UpdateFileAsync(int id ,FileUpdateDTO fileUpdateDTO, ClaimsPrincipal user);
        public Task<QueryResultDTO<FileResponseDTO>> GetAllFilesAsync(QueryParametersDTO parameters);
        public Task<FileResponseDTO> GetFileAsync(int id);


    }
}
