using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public class ProjectDocumentRepositoryProfile : Profile
    {
        public ProjectDocumentRepositoryProfile()
        {// Convert DateTime -> DateOnly
            CreateMap<DateTime, DateOnly>().ConvertUsing(src => DateOnly.FromDateTime(src));
            CreateMap<DateTime?, DateOnly?>().ConvertUsing(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : (DateOnly?)null);

            // Convert DateOnly -> DateTime
            CreateMap<DateOnly, DateTime>().ConvertUsing(src => src.ToDateTime(TimeOnly.MinValue));
            CreateMap<DateOnly?, DateTime?>().ConvertUsing(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);

            // Entity => ReadDto (DateOnly => DateTime)
            CreateMap<PerformanceReview, PerformanceReviewReadDto>()
                .ForMember(dest => dest.ReviewDate,
                           opt => opt.MapFrom(src => src.ReviewDate.HasValue
                               ? src.ReviewDate.Value.ToDateTime(TimeOnly.MinValue)
                               : (DateTime?)null));

            // CreateDto => Entity (DateTime => DateOnly)
            CreateMap<PerformanceReviewCreateDto, PerformanceReview>()
                .ForMember(dest => dest.ReviewDate,
                           opt => opt.MapFrom(src => src.ReviewDate.HasValue
                               ? DateOnly.FromDateTime(src.ReviewDate.Value)
                               : (DateOnly?)null));

            // UpdateDto => Entity (DateTime => DateOnly)
            CreateMap<PerformanceReviewUpdateDto, PerformanceReview>()
                .ForMember(dest => dest.ReviewDate,
                           opt => opt.MapFrom(src => src.ReviewDate.HasValue
                               ? DateOnly.FromDateTime(src.ReviewDate.Value)
                               : (DateOnly?)null));
        }
    }
    
}
