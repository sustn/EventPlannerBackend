using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace EventPlanner.WebAPI.Middlewares.Options;
public class HeaderOptions : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Tenant-ID",
            In = ParameterLocation.Header,
            Required = false,
            Description = "Tenant ID",
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}
