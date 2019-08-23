using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly IAuthorizationRepository _authRepository;

        public CustomAuthorizationMiddleware(RequestDelegate next, IMemoryCache cache, IAuthorizationRepository authRepository)
        {
            _next = next;
            _cache = cache;
            _authRepository = authRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var claims = new List<Claim>();
                var user = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                if(!_cache.TryGetValue(user, out IEnumerable<string> roles))
                {
                    roles = _authRepository.GetRoles(user);

                    if (roles.Any())
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            // Keep in cache for this time, reset time if accessed.
                            .SetSlidingExpiration(TimeSpan.FromHours(1));

                        _cache.Set(user, roles, cacheEntryOptions);
                    }
                }

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
