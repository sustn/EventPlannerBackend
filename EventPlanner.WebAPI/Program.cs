using EventPlanner.Application;
using EventPlanner.Persistence;
using EventPlanner.WebAPI.Filters;
using EventPlanner.WebAPI.Middlewares;
using EventPlanner.WebAPI.Middlewares.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
}); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.MapType<string>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" });
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Event Planner API", Version = "v1" });
    x.OperationFilter<HeaderOptions>();
});
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAppplication();
var app = builder.Build();
app.UseMiddleware<TenantMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 
app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
