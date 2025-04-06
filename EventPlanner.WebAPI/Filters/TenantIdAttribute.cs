using Microsoft.AspNetCore.Mvc.Filters;

namespace EventPlanner.WebAPI.Filters;

public class TenantIdAttribute:Attribute,IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantIdHeader) && Guid.TryParse(tenantIdHeader.ToString(), out var tenantId))
        {
            context.ActionArguments["tenantId"] = tenantId;
        }
        else
        {
            throw new ArgumentException("Tenant id not found in header");
        }
        await next();
    }
}
