using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur;
using Infraestructur.Data; // Ensure this matches your ApplicationDbContext namespace
using Infraestructur.Identity.Models;
using Infraestructur.Identity.Services;
using Infraestructur.Models;
using Infraestructur.Repositories;
using Infrastructur.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using System.Reflection;

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
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Auth Services
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

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

builder.Services.AddIdentity<UserCredential, AppRole>(options => {

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
    // Usamos las constantes oficiales para evitar errores de dedo (typos)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = jwtSettings["Key"];

    if (string.IsNullOrEmpty(key))
        throw new InvalidOperationException("JWT Key is missing in appsettings.json");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Opcional: elimina el margen de 5 min por defecto
    };
});

// Repositories
builder.Services.AddScoped<IStopRepository, StopRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
// Program.cs o su método de extensión de servicios
builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();

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