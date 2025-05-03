using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IMapper _mapper;

        public FolderController(IFolderService folderService, IMapper mapper) {
            this._folderService = folderService;
            this._mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFolder(FolderCreateDTO folderCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); 

            var folder = _mapper.Map<ProjectFolder>(folderCreateDTO);

            var projectFolderResult = await _folderService.CreateFolder(folder);

            var response = _mapper.Map <FolderResponseDTO>(projectFolderResult);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolder([FromRoute] int id)
        {
            var folder = await _folderService.GetByIdWithDetailsAsync(id);

            var folderDto = _mapper.Map<FolderResponseDTO>(folder);

            return Ok(folderDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFolders([FromQuery] QueryParametersDTO queryParametersDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var queryParameters = _mapper.Map<QueryParameters>(queryParametersDTO);

            var result = await _folderService.GetAllFolders(queryParameters);

            var dtoResult = _mapper.Map<QueryResultDTO<FolderResponseDTO>>(result);

            return Ok(dtoResult);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder([FromRoute] int id)
        {
            var folder = await _folderService.GetByIdWithDetailsAsync(id);

            await _folderService.DeleteFolderAsync(id);

            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateFolder([FromBody] FolderUpdateDTO folderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFolder = await _folderService.GetByIdWithDetailsAsync(folderDto.Id);

            _mapper.Map(folderDto, existingFolder);
            var updatedFolder = await _folderService.UpdateFolder(existingFolder);

            var responseDto = _mapper.Map<FolderResponseDTO>(updatedFolder);
            return Ok(responseDto);
        }


    }
}
