using LocationService;
using LocationService.Data;
using LocationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Location",
        Version = "v1"
    });
});
// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region GRPC
builder.Services.AddGrpc();
// Map the gRPC service
#endregion

if (builder.Environment.EnvironmentName == "Docker")
{
	builder.WebHost.UseUrls("http://*:80");
}
var app = builder.Build();

app.MapGrpcService<EmployeeLocationServiceImp>();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "Location"); });
//}
app.UseRouting();
//app.MapGrpcService<LocationServiceImpl>();
app.MapControllers();

app.Run();
