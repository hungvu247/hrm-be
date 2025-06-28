using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace human_resource_management.Service
{
    public class PasswordHashHandler
    {
        private static readonly int _iterationCount = 10000;

        public static string HashPassword(string password)
        {
            int saltSize = 128 / 8; // 16 bytes
            var salt = new byte[saltSize];
            RandomNumberGenerator.Fill(salt); // random salt mỗi lần hash

            var subkey = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: _iterationCount,
                numBytesRequested: 32); // 256-bit key

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
                
           
            outputBytes[0] = 0x01;
            WriteNetWorkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetWorkByteOrder(outputBytes, 5, (uint)_iterationCount);
            WriteNetWorkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + salt.Length, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var hashPasswordBytes = Convert.FromBase64String(hashedPassword);

                if (hashPasswordBytes.Length < 13)
                    return false;

                var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashPasswordBytes, 1);
                var iterationCount = (int)ReadNetworkByteOrder(hashPasswordBytes, 5);
                var saltSize = (int)ReadNetworkByteOrder(hashPasswordBytes, 9);

                if (saltSize < 128 / 8 || saltSize > 256 / 8 || hashPasswordBytes.Length < 13 + saltSize)
                    return false;

                var salt = new byte[saltSize];
                Buffer.BlockCopy(hashPasswordBytes, 13, salt, 0, saltSize);

                var subkeyLength = hashPasswordBytes.Length - 13 - saltSize;
                if (subkeyLength <= 0)
                    return false;

                var expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashPasswordBytes, 13 + saltSize, expectedSubkey, 0, subkeyLength);

                var actualSubkey = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: prf,
                    iterationCount: iterationCount,
                    numBytesRequested: expectedSubkey.Length);

                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        private static void WriteNetWorkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset]) << 24)
                 | ((uint)(buffer[offset + 1]) << 16)
                 | ((uint)(buffer[offset + 2]) << 8)
                 | ((uint)(buffer[offset + 3]));
        }
    }
}
