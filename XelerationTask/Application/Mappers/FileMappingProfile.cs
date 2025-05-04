using AutoMapper;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Mappers
{
    public class FileMappingProfile : Profile
    {
        public FileMappingProfile() {
        
            CreateMap<FileCreateDTO, ProjectFile>()
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId));

            CreateMap<ProjectFile, FileResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentFolder, opt => opt.MapFrom(src => src.ParentFolder == null ? null : new IdNameDto { Id = src.ParentFolder.Id, Name = src.ParentFolder.Name }))
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.OriginalName))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));

            CreateMap<FileUpdateDTO, ProjectFile>()
                .ForMember(dest => dest.OriginalName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        }
       
    }
}
