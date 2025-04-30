using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            // extra mapping here to return ResponseDTO 

            return Ok(projectFolderResult);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FolderResponseDTO>> GetFolder([FromRoute] int id)
        {
            var folder = await _folderService.GetByIdWithDetailsAsync(id);

            if (folder == null)
                return NotFound("No File with such id found");  // custom exception and maybe moving it to service later 

            var folderDto = _mapper.Map<FolderResponseDTO>(folder);

            return Ok(folderDto);
        }

    }
}
