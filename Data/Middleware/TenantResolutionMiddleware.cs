using Microsoft.AspNetCore.Http;
using SolarService.Interface;
using System.Threading.Tasks;

namespace SolarService.Data.Middleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            // Swagger / static files bypass
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (
                path.StartsWith("/swagger") ||
                path.StartsWith("/favicon") ||
                path.StartsWith("/health") ||
                path.Contains("index.html")
            ))
            {
                tenantProvider.SetTenant(new Models.Tenant { Id = 1 });
                await _next(context);
                return;
            }

            // Read from header
            if (context.Request.Headers.TryGetValue("x-tenant-id", out var tenantIdValue))
            {
                if (int.TryParse(tenantIdValue, out int tenantId))
                {
                    tenantProvider.SetTenant(new Models.Tenant { Id = tenantId });
                }
                else
                {
                    tenantProvider.SetTenant(new Models.Tenant { Id = 1 }); // fallback
                }
            }
            else
            {
                // DEFAULT Tenant
                tenantProvider.SetTenant(new Models.Tenant { Id = 1 });
            }

            await _next(context);
        }
    }
}
