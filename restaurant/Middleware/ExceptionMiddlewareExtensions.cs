// File: Middlewares/ExceptionMiddlewareExtensions.cs
using Microsoft.AspNetCore.Builder;

namespace restaurant.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
