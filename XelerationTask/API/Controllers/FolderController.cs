using AutoMapper;
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


        [HttpPost]
        public async Task<ActionResult> CreateFolder(FolderCreateDTO folderCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); 

            var folder = _mapper.Map<ProjectFolder>(folderCreateDTO);

            var projectFolderResult = await _folderService.CreateFolder(folder);

            var response = _mapper.Map <FolderResponseDTO>(projectFolderResult);

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FolderResponseDTO>> GetFolder([FromRoute] int id)
        {
            var folder = await _folderService.GetByIdWithDetailsAsync(id);

            var folderDto = _mapper.Map<FolderResponseDTO>(folder);

            return Ok(folderDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFolder([FromRoute] int id)
        {
            var folder = await _folderService.GetByIdWithDetailsAsync(id);

            await _folderService.DeleteFolderAsync(id);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<FolderResponseDTO>> UpdateFolder([FromBody] FolderUpdateDTO folderDto)
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
