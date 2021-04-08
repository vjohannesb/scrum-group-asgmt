using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data.ViewModels;

namespace WebAPI.Services
{
    public interface IIdentityService
    {
        public Task<ResponseModel> SignInAsync(SignInModel model);
    }
}
