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
            await CleanUpExpiredRefreshTokens();
            if (string.IsNullOrWhiteSpace(requestModel.Username) || string.IsNullOrWhiteSpace(requestModel.Password))
                return null;
            var user = await _humanResourceManagementContext.Employees
                .FirstOrDefaultAsync(u => u.Username == requestModel.Username);

            if (user is null || !PasswordHashHandler.VerifyPassword(requestModel.Password, user.Password))
                return null;
            var roleName = await _humanResourceManagementContext.Roles
       .Where(r => r.RoleId == user.RoleId)
       .Select(r => r.RoleName)
       .FirstOrDefaultAsync();

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenExpirationInMinutes");
            var accessTokenExpiry = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            // 0. THU HỒI TOÀN BỘ refresh token cũ chưa dùng
            var oldTokens = await _humanResourceManagementContext.RefreshTokens
                .Where(t => t.EmployeeId == user.EmployeeId && !t.IsRevoked && !t.IsUsed)
                .ToListAsync();

            foreach (var token in oldTokens)
            {
                token.IsUsed = true;
                token.IsRevoked = true;
            }
            await _humanResourceManagementContext.SaveChangesAsync();

            // 1. Tạo AccessToken
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("EmployeeId", user.EmployeeId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
                Expires = accessTokenExpiry,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            var jwtId = securityToken.Id;

            // 2. Tạo Refresh Token
            var refreshToken = new RefreshToken
            {
                JwtId = jwtId,
                Token = Guid.NewGuid().ToString(),
                EmployeeId = user.EmployeeId,
                IsUsed = false,
                IsRevoked = false,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7) // hoặc .AddSeconds(30) nếu muốn test
            };

            // 3. Lưu Refresh Token vào DB
            await _humanResourceManagementContext.RefreshTokens.AddAsync(refreshToken);
            await _humanResourceManagementContext.SaveChangesAsync();

            // 4. Trả kết quả
            return new LoginResponseModel
            {
                EmployeeId = user.EmployeeId,
                PositionId = user.PositionId,
                RoleId = user.RoleId,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = (int)(accessTokenExpiry - DateTime.UtcNow).TotalSeconds,
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
        public async Task<LoginResponseModel?> RefreshToken(TokenRequestModel model)
        {
            await CleanUpExpiredRefreshTokens();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(model.AccessToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtConfig:Issuer"],
                    ValidAudience = _configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]))
                }, out SecurityToken validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                var jwtId = jwtToken.Id;
                var username = principal.Identity.Name;

                // Kiểm tra RefreshToken trong DB
                var storedToken = await _humanResourceManagementContext.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == model.RefreshToken);

                if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow || storedToken.JwtId != jwtId)
                    return null;

                // Đánh dấu token cũ đã dùng
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                _humanResourceManagementContext.RefreshTokens.Update(storedToken);
                await _humanResourceManagementContext.SaveChangesAsync();

                // Tìm lại user
                var user = await _humanResourceManagementContext.Employees
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                    return null;

                // Sinh token mới
                var loginModel = new LoginRequestModel
                {
                    Username = user.Username,
                    Password = "" // không cần vì ta đã xác thực qua refresh token rồi
                };

                var result = await Authenticate(loginModel); // dùng lại hàm bạn đã có
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task CleanUpExpiredRefreshTokens()
        {
            var now = DateTime.UtcNow;
            var expiredTokens = await _humanResourceManagementContext.RefreshTokens
                .Where(t => t.ExpiryDate < now)
                .ToListAsync();

            Console.WriteLine($"UTC Now: {now}");
            foreach (var token in expiredTokens)
            {
                Console.WriteLine($"Xóa token {token.Token} hết hạn lúc {token.ExpiryDate}");
            }

            if (expiredTokens.Any())
            {
                _humanResourceManagementContext.RefreshTokens.RemoveRange(expiredTokens);
                await _humanResourceManagementContext.SaveChangesAsync();
            }
        }
        public string? GetUsernameFromToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Không check hết hạn, chỉ parse ra
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtConfig:Issuer"],
                    ValidAudience = _configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]))
                }, out SecurityToken validatedToken);

                return principal.Identity?.Name;
            }
            catch
            {
                return null;
            }
        }
        public int? GetEmployeeIdFromToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Không check hết hạn khi đọc
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtConfig:Issuer"],
                    ValidAudience = _configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]))
                }, out SecurityToken validatedToken);

                var employeeIdClaim = principal.FindFirst("EmployeeId");
                if (employeeIdClaim != null && int.TryParse(employeeIdClaim.Value, out int employeeId))
                {
                    return employeeId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


    }
}