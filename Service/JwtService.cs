using Azure.Core;
using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace human_resource_management.Service
{
    public class JwtService
    {
        private HumanResourceManagementContext _humanResourceManagementContext;
        private IConfiguration _configuration;
        public JwtService(HumanResourceManagementContext humanResourceManagementContext, IConfiguration configuration)
        {
            _humanResourceManagementContext = humanResourceManagementContext;
            _configuration = configuration;
        }
        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel requestModel)
        {
            if (string.IsNullOrWhiteSpace(requestModel.Username) || string.IsNullOrWhiteSpace(requestModel.Password))
            {
                return null;
            }


            var user = await _humanResourceManagementContext.Employees
                .FirstOrDefaultAsync(u => u.Username == requestModel.Username);


            if (user is null || !PasswordHashHandler.VerifyPassword(requestModel.Password, user.Password))
            {
                return null;
            }


            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenExpirationInMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            // Tạo token
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Name, user.Username)
                     }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"])),
                                    SecurityAlgorithms.HmacSha256Signature
                 )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDetails);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                AccessToken = accessToken,
                ExpiresIn = (int)(tokenExpiryTimeStamp - DateTime.UtcNow).TotalSeconds,
                UserName = user.Username
            };
        }
        public async Task HashAllPlainTextPasswords()
        {

            var users = await _humanResourceManagementContext.Employees
                .Where(u => u.Password.Length < 20)
                .ToListAsync();

            foreach (var user in users)
            {
                string oldPassword = user.Password;


                string hashedPassword = PasswordHashHandler.HashPassword(oldPassword);


                user.Password = hashedPassword;
            }

            await _humanResourceManagementContext.SaveChangesAsync();
        }


    }
}