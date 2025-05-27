using AuthService.Persistence;
using AuthService.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
	.AddJsonFile("appsettings.json", optional: false)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
	.AddEnvironmentVariables();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<ITokenService, TokenService>();

// Add DbContext for Identity and your Application DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    



// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5000"; // Your IdentityServer URL
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			// Ensure the key is at least 128 bits long
			IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:SecretKey"])) // Base64 128-bit key
        };
    });


// Add Redis configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Redis server address, replace with your Redis server URL if using cloud Redis
    options.InstanceName = "AuthService:";
});



// Add Controllers
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen();

if (builder.Environment.EnvironmentName == "Docker")
{
	builder.WebHost.UseUrls("http://*:80");
}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Swagger Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API V1");
    c.RoutePrefix = ""; // Optional: makes Swagger available at the root URL
});

// Use Routing (this needs to be called before UseAuthentication and UseAuthorization)
app.UseRouting();


app.UseAuthentication();  // Authentication must come before Authorization
app.UseAuthorization();   // Authorization must be after Authentication

// Map Controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
