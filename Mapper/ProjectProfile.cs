using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Model;
namespace human_resource_management.Mapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectUpdateDto, Project>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? DateOnly.FromDateTime(src.StartDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? DateOnly.FromDateTime(src.EndDate.Value) : (DateOnly?)null));

            CreateMap<ProjectCreateDto, Project>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? DateOnly.FromDateTime(src.StartDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? DateOnly.FromDateTime(src.EndDate.Value) : (DateOnly?)null));

            CreateMap<Project, ProjectReadDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? src.StartDate.Value.ToDateTime(TimeOnly.MinValue) : default))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? src.EndDate.Value.ToDateTime(TimeOnly.MinValue) : default));
        }
    }
}
