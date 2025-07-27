using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public class RequestFormProfile : Profile
    {
        public RequestFormProfile()
        {
            CreateMap<RequestForm, RequestFormDto>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FirstName +" " + src.Employee.LastName))
            .ForMember(dest => dest.RequestTypeName,
                opt => opt.MapFrom(src => src.RequestType.RequestTypeName))
            .ForMember(dest => dest.ReviewedByName,
                opt => opt.MapFrom(src => src.ReviewedByNavigation != null
                    ? src.ReviewedByNavigation.FirstName+ " " + src.ReviewedByNavigation.LastName
                    : null))
            .ForMember(dest => dest.ReviewedAt,
                opt => opt.MapFrom(src => src.ReviewedAt))
            .ForMember(dest => dest.ReviewComment,
                opt => opt.MapFrom(src => src.ReviewComment));
            CreateMap<CreateRequestFormDto, RequestForm>();
            CreateMap<UpdateStatusDto, RequestForm>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
