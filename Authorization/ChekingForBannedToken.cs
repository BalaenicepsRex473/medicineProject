using Microsoft.EntityFrameworkCore;
using System;

namespace scrubsAPI.Authorization
{
    public class CheckingForBannedToken
    {
        private readonly RequestDelegate _next;

        public CheckingForBannedToken(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var dbContext = context.RequestServices.GetRequiredService<ScrubsDbContext>();

                var blockedToken = await dbContext.BannedTokens
                    .FirstOrDefaultAsync(bt => bt.token == token);

                if (blockedToken != null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("This session is out");
                    return;
                }
            }

            await _next(context);
        }
    }
}
