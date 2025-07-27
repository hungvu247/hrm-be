namespace human_resource_management.Dto
{
   

    public class RequestFormDto
    {
        public int RequestId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public string EmployeeName { get; set; }
        public string RequestTypeName { get; set; }
        public string ReviewedByName { get; set; }

        public DateTime? ReviewedAt { get; set; }
        public string ReviewComment { get; set; }
    }

    public class CreateRequestFormDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int RequestTypeId { get; set; }
    }
    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

}
