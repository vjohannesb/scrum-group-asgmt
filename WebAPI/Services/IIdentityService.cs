using SharedLibrary.Models.ViewModels;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface IIdentityService
    {
        public Task<ResponseModel> SignInAsync(SignInModel model);
    }
}
