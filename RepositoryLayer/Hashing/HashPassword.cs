using System;
using System.Security.Cryptography;

namespace RepositoryLayer.Hashing
{
    public class HashPassword
    {
        private const int SaltSize = 16;  
        private const int HashSize = 20;  
        private const int Iterations = 10000;

        public string PasswordHashing(string userPass)
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt); 

            using var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashByte = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashByte, 0, SaltSize);
            Array.Copy(hash, 0, hashByte, SaltSize, HashSize);

            return Convert.ToBase64String(hashByte);
        }

        public bool VerifyPassword(string userPass, string storedHashPass)
        {
            byte[] hashByte = Convert.FromBase64String(storedHashPass);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashByte, 0, salt, 0, SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, hashByte.AsSpan(SaltSize, HashSize));
        }
    }
}
