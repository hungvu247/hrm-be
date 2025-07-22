namespace human_resource_management.Model
{
    public class LoginResponseModel
    {
        public int EmployeeId { get; set; }
        public string AccessToken { get; set; }
        public int? PositionId { get; set; }
        public int? RoleId { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string UserName { get; set; }
    }
}