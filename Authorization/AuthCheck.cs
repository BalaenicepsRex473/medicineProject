using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;

namespace scrubsAPI
{
    public class AuthCheck
    {
        private readonly RequestDelegate _next;
        private readonly TokenStorage _tokenStorage;
        public AuthCheck(RequestDelegate next, TokenStorage tokenStorage)
        {
            _next = next;
            _tokenStorage = tokenStorage;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (context.Request.Headers.ContainsKey("Authorization"))
            {

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!_tokenStorage.TokenExists(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
                    return;
                }
                
            }
            await _next(context);
        }
    }
}
