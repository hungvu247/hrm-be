using human_resource_management.Util;

namespace human_resource_management.Dto
{
    public class CreateEmployeeContactDto
    {
        public int EmployeeId { get; set; }
        public ContactTypeEnum ContactType { get; set; }
        public string ContactValue { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
