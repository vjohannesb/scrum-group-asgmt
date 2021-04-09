using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace WebAPI.Data
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] Token { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }


        public void CreatePasswordWithHash(string password)
        {
            using var hmac = new HMACSHA512();
            PasswordSalt = hmac.Key;
            Password = GenerateSaltedHash(Encoding.UTF8.GetBytes(password));
        }
        private byte[] GenerateSaltedHash(byte[] password)
        {
            using var hmac = new HMACSHA512(PasswordSalt);
            byte[] saltedPass = password.Concat(PasswordSalt).ToArray();
            return hmac.ComputeHash(saltedPass);
        }
    

    }
}
