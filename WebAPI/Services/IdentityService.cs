using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Data.ViewModels;

namespace WebAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SqlDbContext _context;

        public IdentityService(SqlDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> SignInAsync(SignInModel model)
        {
            if (!EmailRegistered(model.Email))
                return new ResponseModel(false, null);

            var customer = await _context.Customers.FirstOrDefaultAsync(customer => customer.Email == model.Email);

            if (!customer.ValidatePasswordHash(model.Password))
                return new ResponseModel(false, null);

            // TOKEN
            // TOKEN

            return new ResponseModel(true, "Logged in!");
        }

        private bool EmailRegistered(string email) =>
            _context.Customers.Any(customer => customer.Email == email);
    }
}
