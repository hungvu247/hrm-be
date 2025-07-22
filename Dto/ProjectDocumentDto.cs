namespace human_resource_management.Dto
{
    public class ProjectDocumentDto
    {
    }
    public class ProjectDocumentCreateDto
    {
        public int ProjectId { get; set; }
        public string? DocumentName { get; set; }
        public string? FilePath { get; set; }
        public DateOnly? UploadDate { get; set; }
    }

    public class ProjectDocumentReadDto
    {
        public int DocumentId { get; set; }
        public int ProjectId { get; set; }
        public string? DocumentName { get; set; }
        public string? FilePath { get; set; }
        public DateOnly? UploadDate { get; set; }
    }

    public class ProjectDocumentUpdateDto
    {
        public string? DocumentName { get; set; }
        public string? FilePath { get; set; }
        public DateOnly? UploadDate { get; set; }
    }
}
