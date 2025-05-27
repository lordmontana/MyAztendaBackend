using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product",
        Version = "v1"
    });
});
// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

if (builder.Environment.EnvironmentName == "Docker")
{
	builder.WebHost.UseUrls("http://*:80");
}
var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "Product"); });

//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
