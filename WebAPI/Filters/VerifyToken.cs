using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifyToken : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorized = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer);
            if (!authorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = bearer.ToString().Split(" ")[1];
            var id = GetUserIdFromToken(token);

            if (id < 1)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<SqlDbContext>();
            var customer = await dbContext.Customers.FindAsync(id);

            if (customer == null || customer?.Token == null || !customer.ValidateTokenHash(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

        private static int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var parsed = int.TryParse(tokenId, out int id);

            return parsed ? id : 0;
        }
    }
}
