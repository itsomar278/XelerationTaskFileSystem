using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Application.Services;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;

    public FolderController(IFolderService folderService) => _folderService = folderService;

    [HttpPost("create")]
    public async Task<IActionResult> CreateFolder(FolderCreateDTO folderCreateDTO) =>
        Ok(await _folderService.CreateFolder(folderCreateDTO, User));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFolder([FromRoute] int id) =>
        Ok(await _folderService.GetFolderAsync(id));

    [HttpPost("All")]
    public async Task<IActionResult> GetFolders([FromBody] QueryParametersDTO queryParametersDTO) =>
        Ok(await _folderService.GetAllFolders(queryParametersDTO));

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteFolder([FromRoute] int id)=>
        await _folderService.DeleteFolderAsync(id, User).ContinueWith(_ => NoContent());


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFolder([FromBody] FolderUpdateDTO folderDto, [FromRoute] int id) =>
        Ok(await _folderService.UpdateFolder(id, folderDto, User));
}
