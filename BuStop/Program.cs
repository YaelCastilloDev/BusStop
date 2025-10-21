// Program.cs (Updated)

using Infraestructur.Models;

var builder = WebApplication.CreateBuilder(args);


var licenseKey = builder.Configuration.GetValue<string>("AutoMapper:LicenseKey");


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. Configure AutoMapper with the retrieved Key and Assembly Scan ---
// Use the overload that takes a delegate (Action<IMapperConfigurationExpression>)
// to set the LicenseKey, AND THEN use .AddAutoMapper(params Assembly[])
// or a combined approach.

// The most common approach is to use two steps or use the overload that accepts types:

// Step A: Register AutoMapper and configure it (License Key)
// Program.cs

// Assuming licenseKey is already defined and loaded from configuration.

builder.Services.AddAutoMapper(cfg =>
{
    // 1. Configuration Delegate: Set the license key
    if (!string.IsNullOrEmpty(licenseKey))
    {
        cfg.LicenseKey = licenseKey;
    }

    // 2. Profile Registration: Manually add the profile within the delegate
    cfg.AddProfile(new IdentityMappingProfile());

    // NOTE: If IdentityMappingProfile is in a separate assembly (which it is, in Infrastructure),
    // you must ensure the application project has a reference to that assembly for 
    // 'new IdentityMappingProfile()' to compile and for the runtime to find it.

    // A slightly better way that uses the built-in scanning:
    // cfg.AddMaps(typeof(IdentityMappingProfile).Assembly);

}, typeof(IdentityMappingProfile)); // <-- Optionally pass the profile type here for AutoMapper to scan its assembly

builder.Services.AddMemoryCache();

var app = builder.Build();

// ... rest of the code

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
 //   app.UseSwagger();
 //   app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();