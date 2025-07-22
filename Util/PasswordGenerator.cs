using BCrypt.Net;
using human_resource_management.Dto;
using Microsoft.Extensions.Options;

namespace human_resource_management.Util
{
    public class PasswordGenerator
    {
        private readonly PasswordSettings _settings;

        public PasswordGenerator(IOptions<PasswordSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateRandomPassword(int? length = null)
        {
            int passLength = length ?? _settings.DefaultLength;
            string validChars = _settings.ValidChars;

            var random = new Random();
            return new string(Enumerable.Repeat(validChars, passLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

     
    }
}
