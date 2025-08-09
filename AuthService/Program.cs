using AuthService.Models;
using AuthService.Persistence;
using AuthService.Services;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<JwtSettingsModel>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();
builder.Services.AddScoped<IUserService, AuthService.Services.AuthService>();
builder.Services.AddScoped<INotificationService, AuthService.Services.AuthService>();

// DbContext (PostgreSQL)
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgsql =>
    {
        npgsql.EnableRetryOnFailure();
        npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "atzenda");
    })
       .UseSnakeCaseNamingConvention()
);

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // tweak if you have specific password/lockout requirements
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT auth (reads your Jwt:Issuer/Audience/SecretKey from appsettings)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(opts =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
    opts.AddPolicy("AllowFrontend", p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

if (builder.Environment.EnvironmentName == "Docker")
    builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API V1");
        c.RoutePrefix = "";
    });
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // simpler than UseEndpoints in .NET 8

app.Run();
