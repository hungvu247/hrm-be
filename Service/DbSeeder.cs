using human_resource_management.Model;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Service
{
    public class DbSeeder
    {
        private readonly HumanResourceManagementContext _context;

        public DbSeeder(HumanResourceManagementContext context)
        {
            _context = context;
        }

        //public async Task SeedUsersAsync()
        //{
        //    // Nếu DB đã có user, không seed nữa
        //    if (await _context.Employees.AnyAsync())
        //        return;

        //    var users = new List<User>
        //{
        //    new User { Username = "admin", Password = PasswordHashHandler.HashPassword("Admin123!"), Email = "admin@example.com", RoleId = 1 },
        //    new User { Username = "john", Password = PasswordHashHandler.HashPassword("John123!"), Email = "john@example.com", RoleId = 2 },
        //    new User { Username = "jane", Password = PasswordHashHandler.HashPassword("Jane123!"), Email = "jane@example.com", RoleId = 2 },
        //    new User { Username = "bob", Password = PasswordHashHandler.HashPassword("Bob123!"), Email = "bob@example.com", RoleId = 3 },
        //    new User { Username = "alice", Password = PasswordHashHandler.HashPassword("Alice123!"), Email = "alice@example.com", RoleId = 3 }
        //};

        //    _context.Employees.AddRange(users);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task FixPlainTextPasswordsAsync()
        //{
        //    var users = await _context.Users
        //        .Where(u => u.Password.Length < 30) // chỉ lấy những người chưa hash
        //        .ToListAsync();

        //    foreach (var user in users)
        //    {
        //        // ⚠️ Nếu bạn không biết mật khẩu gốc là gì, bạn cần đặt lại mặc định
        //        user.Password = PasswordHashHandler.HashPassword("123456");
        //    }

        //    await _context.SaveChangesAsync();
        //}

    }
}
