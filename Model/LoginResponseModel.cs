namespace human_resource_management.Model
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string UserName { get; set; }
    }
}