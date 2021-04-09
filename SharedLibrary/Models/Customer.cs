using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace SharedLibrary.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

        public int CustomerId { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public void CreatePasswordWithHash(string password)
        {
            using var hmac = new HMACSHA512();
            PasswordSalt = hmac.Key;
            Password = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
        }

        public bool ValidatePasswordHash(string password)
        {
            var saltedHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
            if (saltedHash.Length != Password.Length)
                return false;

            for (var i = 0; i < saltedHash.Length; i++)
                if (saltedHash[i] != Password[i])
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
