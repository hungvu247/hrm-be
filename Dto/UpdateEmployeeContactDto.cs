using human_resource_management.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace human_resource_management.Dto
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateEmployeeContactDto
    {
        public ContactTypeEnum? ContactType { get; set; }
        public string? ContactValue { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
