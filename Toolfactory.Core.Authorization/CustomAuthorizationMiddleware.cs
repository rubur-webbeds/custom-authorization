using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Toolfactory.Core.Authorization
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthorizationRepository _authRepository;

        public CustomAuthorizationMiddleware(RequestDelegate next, IAuthorizationRepository authRepository)
        {
            _next = next;
            _authRepository = authRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var claims = new List<Claim>();
                var user = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var roles = _authRepository.GetRoles(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                context.User.AddIdentity(new ClaimsIdentity(claims));
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
