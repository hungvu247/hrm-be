namespace human_resource_management.Util
{
    using BCrypt.Net;

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashed)
        {
            return BCrypt.Verify(password, hashed);
        }
    }

}
