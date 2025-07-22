using AutoMapper;
using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public class ProjectDocumentProfile :Profile
    {
        public ProjectDocumentProfile()
        {
            CreateMap<ProjectDocument, ProjectDocumentReadDto>();
            CreateMap<ProjectDocumentCreateDto, ProjectDocument>();
            CreateMap<ProjectDocumentUpdateDto, ProjectDocument>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
