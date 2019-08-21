using ASPNetCoreJWTSample.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASPNetCoreJWTSample.Middleware
{
    public class CustomAuthMiddleware<T> where T : IAuthRepository, new()
    {
        private readonly RequestDelegate _next;
        private readonly T _rolesRepo;

        public CustomAuthMiddleware(RequestDelegate next)
        {
            _rolesRepo = new T();
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var roles = _rolesRepo.GetRoles(user);
            //var roles = new List<Claim> { new Claim(ClaimTypes.Role, "Accountant") };
            //context.User.AddIdentity(new ClaimsIdentity(roles));

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
