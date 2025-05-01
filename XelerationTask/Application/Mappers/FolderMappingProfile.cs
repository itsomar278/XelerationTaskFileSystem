using AutoMapper;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Mappers
{
    public class FolderMappingProfile : Profile
    {
        public FolderMappingProfile() {

            CreateMap<FolderCreateDTO, ProjectFolder>();


            CreateMap<ProjectFolder, FolderResponseDTO>()
           .ForMember(dest => dest.ParentFolder, opt => opt.MapFrom(src =>src.ParentFolder == null ? null : new IdNameDto { Id = src.ParentFolder.Id, Name = src.ParentFolder.Name }))
           .ForMember(dest => dest.ChildFolders, opt => opt.MapFrom(src => src.SubFolders.Select(f => new IdNameDto { Id = f.Id, Name = f.Name }).ToList()))
           .ForMember(dest => dest.ChildFiles, opt => opt.MapFrom(src =>src.Files.Select(f => new IdNameDto { Id = f.Id, Name = f.Name }).ToList()))
           .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));



            CreateMap<FolderUpdateDTO, ProjectFolder>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        }
    }
}
