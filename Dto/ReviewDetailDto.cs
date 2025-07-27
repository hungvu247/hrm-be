namespace human_resource_management.Dto
{
    public class ReviewDetailDto
    {
        public int ReviewId { get; set; }
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public DateOnly? ReviewDate { get; set; }
        public string Comments { get; set; }
    }

}
