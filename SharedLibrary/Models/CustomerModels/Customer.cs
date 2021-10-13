using System;
using System.Collections.Generic;
using SharedLibrary.Models.ProductModels;
using SharedLibrary.Models.OrderModels;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

#nullable disable

namespace SharedLibrary.Models.CustomerModels
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int CustomerId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }

        public void CreatePasswordWithHash(string password)
        {
            using var hmac = new HMACSHA512();
            PasswordSalt = hmac.Key;
            PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
        }

        public bool ValidatePasswordHash(string password)
        {
            var saltedHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
            if (saltedHash.Length != PasswordHash.Length)
                return false;

            for (var i = 0; i < saltedHash.Length; i++)
                if (saltedHash[i] != PasswordHash[i])
                    return false;

            return true;
        }

        public void CreateTokenWithHash(string token)
            => AccessToken = GenerateSaltedHash(Encoding.UTF8.GetBytes(token));

        public bool ValidateTokenHash(string token)
        {
            var saltedAccessToken = GenerateSaltedHash(Encoding.UTF8.GetBytes(token));
            if (saltedAccessToken.Length != AccessToken.Length)
                return false;

            for (var i = 0; i < saltedAccessToken.Length; i++)
                if (saltedAccessToken[i] != AccessToken[i])
                    return false;

            return true;
        }

        private byte[] GenerateSaltedHash(byte[] password)
        {
            using var hmac = new HMACSHA512(PasswordSalt);
            byte[] saltedPass = password.Concat(PasswordSalt).ToArray();
            return hmac.ComputeHash(saltedPass);
        }
    }
}