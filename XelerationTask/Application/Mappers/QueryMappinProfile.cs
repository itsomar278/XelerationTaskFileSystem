using AutoMapper;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Application.Mappers
{
    public class QueryMappinProfile : Profile
    {
        public QueryMappinProfile() {

            CreateMap<QueryParametersDTO, QueryParameters>();

            CreateMap(typeof(QueryResult<>), typeof(QueryResultDTO<>))
           .ForMember("Items", opt => opt.MapFrom("Items"))
           .ForMember("TotalCount", opt => opt.MapFrom("TotalCount"))
           .ForMember("Page", opt => opt.MapFrom("Page"))
           .ForMember("PageSize", opt => opt.MapFrom("PageSize"));

        }
    }
}
