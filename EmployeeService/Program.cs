using EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;
using EmployeeService.Cqrs.Commands;  // marker type
using EmployeeService.Logging;
using EmployeeService.Persistence;
using EmployeeService.Services;
using EmployeeService.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Admin.Interfaces;
using Shared.Admin.Services;
using Shared.Cqrs.DependencyInjection;
using Shared.Logging.Extensions;
using Shared.Logging.Interceptors;
using Shared.Logging.Interfaces;
using Shared.Repositories.Abstractions;
using Shared.Repositories.Persistence;
using Shared.Web.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee",
        Version = "v1"
    });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// Add authentication services
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer("Bearer", options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,

			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,

			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Convert.FromBase64String(jwtSettings.SecretKey)),

			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};

		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine($"[EMPLOYEE AUTH FAILED]: {context.Exception.Message}");
				return Task.CompletedTask;
			}
		};
	});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddCqrs(typeof(CreateEmployeeCommand).Assembly);  // scans this service
builder.Services.AddScoped<IUserInfoProvider, UserInfoProvider>();
builder.Services.AddScoped<UserInfoProvider>();

#region GRPC
builder.Services.AddScoped<EmployeeGRPCClientService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var grpcServerUrl = configuration["GrpcServer:EmployeeLocation"];
    return new EmployeeGRPCClientService(grpcServerUrl);
});

builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var grpcServerUrl = configuration["GrpcServer:EmployeeLocation"];
    return Grpc.Net.Client.GrpcChannel.ForAddress(grpcServerUrl);
});

#endregion

if (builder.Environment.EnvironmentName == "Docker")
{
	builder.WebHost.UseUrls("http://*:80");
}
var app = builder.Build();
app.UseGlobalExceptionHandling();
app.UseMiddleware<RawBodyMiddleware>();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "employee"); });
//}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();  // Enable authentication
app.UseAuthorization();   // Enable authorization

app.MapControllers();

app.Run();
