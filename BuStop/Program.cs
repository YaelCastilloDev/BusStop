using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur;
using Infraestructur.Data; // Ensure this matches your ApplicationDbContext namespace
using Infraestructur.Identity.Services;
using Infraestructur.Models;
using Infraestructur.Repositories;
using Infrastructur.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuration & Variables ---
var licenseKey = builder.Configuration.GetValue<string>("AutoMapper:LicenseKey");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// --- 2. Service Registration (MUST BE BEFORE builder.Build()) ---

builder.Services.AddControllers();

// MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Application.Features.Auth.Commands.Register.RegisterUserCommand).Assembly);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auth Services
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    if (!string.IsNullOrEmpty(licenseKey)) cfg.LicenseKey = licenseKey;
    cfg.AddProfile(new IdentityMappingProfile());
}, typeof(IdentityMappingProfile));

builder.Services.AddMemoryCache();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        x => x.UseNetTopologySuite()
    ));

builder.Services.AddIdentity<User, Role>(options =>
{
    // Password settings (optional configuration)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// --- 3. Build the Application ---
var app = builder.Build(); // <--- The "Lock" happens here.

// --- 4. Configure the HTTP request pipeline (Middleware) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();