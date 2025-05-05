using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Interfaces;

namespace XelerationTask.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFile([FromBody] FileCreateDTO fileCreateDTO) =>
            Ok(await _fileService.CreateFileAsync(fileCreateDTO, User));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] int id) =>
            Ok(await _fileService.GetFileAsync(id));

        [HttpGet("All")]
        public async Task<IActionResult> GetFiles([FromBody] QueryParametersDTO queryParametersDTO) =>
            Ok(await _fileService.GetAllFilesAsync(queryParametersDTO));

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id) =>
            await _fileService.DeleteFileAsync(id, User).ContinueWith(_ => NoContent());

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFile([FromBody] FileUpdateDTO fileUpdateDTO , [FromRoute] int id) =>
            Ok(await _fileService.UpdateFileAsync(id ,fileUpdateDTO, User));
    }
}
