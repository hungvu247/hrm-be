namespace human_resource_management.Dto
{
    public class EmployeeContactDto
    {
        public int ContactId { get; set; }
        public int EmployeeId { get; set; }
        public string ContactType { get; set; } = string.Empty;
        public string ContactValue { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

}
