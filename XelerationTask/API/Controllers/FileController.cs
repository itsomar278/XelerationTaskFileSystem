using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public FileController(IMapper mapper, IFileService fileService)
        {
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFile([FromBody] FileCreateDTO fileCreateDTO)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var projectFile = _mapper.Map<ProjectFile>(fileCreateDTO);

            var createdFile = await _fileService.CreateFile(projectFile);

            var fileToReturn = _mapper.Map<FileResponseDTO>(createdFile);

            return Ok(fileToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] int id)
        {
            var file = await _fileService.GetByIdWithDetailsAsync(id);
            var fileToReturn = _mapper.Map<FileResponseDTO>(file);
            return Ok(fileToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles([FromQuery] QueryParametersDTO queryParametersDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var queryParameters = _mapper.Map<QueryParameters>(queryParametersDTO);
            var result = await _fileService.GetAllFilesAsync(queryParameters);
            var filesToReturn = _mapper.Map<QueryResultDTO<FileResponseDTO>>(result);
            return Ok(filesToReturn);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id)
        {
            await _fileService.DeleteFileAsync(id);
            return Ok("File Deleted !");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFile([FromBody] FileUpdateDTO fileUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var projectFile = _mapper.Map<ProjectFile>(fileUpdateDTO);
            var updatedFile = await _fileService.UpdateFile(projectFile);
            var fileToReturn = _mapper.Map<FileResponseDTO>(updatedFile);
            return Ok(fileToReturn);
        }


    }
}
