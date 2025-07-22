namespace human_resource_management.Dto
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string WelcomeTitle { get; set; }
        public string WelcomeMessage { get; set; }
        public string Footer { get; set; }
        public string TemplateFolder { get; set; }
        public string TemplateFileName { get; set; }
    }
}
