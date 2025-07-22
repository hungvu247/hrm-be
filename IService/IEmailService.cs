namespace human_resource_management.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string password);
    }
}
