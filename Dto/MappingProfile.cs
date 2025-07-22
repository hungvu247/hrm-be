using AutoMapper;
using human_resource_management.Model;

namespace human_resource_management.Dto
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<PerformanceReview, PerformanceReviewReadDto>();
            CreateMap<PerformanceReviewCreateDto, PerformanceReview>();
            CreateMap<PerformanceReviewUpdateDto, PerformanceReview>();
        }
    }
}
