using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toolfactory.Core.Authorization
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseToolfactoryCoreAuthorization(this IApplicationBuilder app)
        {
           return app.UseMiddleware<CustomAuthorizationMiddleware>();
        }
    }
}
