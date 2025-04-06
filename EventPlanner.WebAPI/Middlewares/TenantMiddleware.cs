using System.IO;

namespace EventPlanner.WebAPI.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        var path = context.Request.Path.Value;

        if (path != null &&  path.Contains("/swagger/"))
        {
            await _next(context);
            return;
        }
        if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
        {
            context.Items["TenantId"] = tenantId.ToString();
        }
        var tenant = context.Items["TenantId"]?.ToString();
        if (!string.IsNullOrEmpty(tenant) && tenant == "67f0f0eb-7084-8007-8df6-ae7414e2cf52")
        {
            Console.WriteLine("Tenant ID authorized.");
            await _next(context);
        }
        else
        {
            Console.WriteLine("Unauthorized tenant.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Unauthorized",
                Errors = new List<string> { "Unauthorized or missing X-Tenant-ID" }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }



}
