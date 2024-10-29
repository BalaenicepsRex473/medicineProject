using Microsoft.EntityFrameworkCore;
using scrubsAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System;
using Microsoft.OpenApi.Models;
using scrubsAPI.Schemas;
using scrubsAPI.Authorization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ScrubsDbContext>(
               options => options.UseSqlite("Data Source=Application.db;Cache=Shared"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    }
);

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    config.OperationFilter<SwaggerAuthorizeFilter>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CheckingForBannedToken>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
