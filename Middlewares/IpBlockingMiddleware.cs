public class IpBlockingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public IpBlockingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var clsAccessList = scope.ServiceProvider.GetRequiredService<Event_managment.BL.ClsAccessList>();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // Check if the IP is blocked
            if (ipAddress != null && clsAccessList.IsBlocked(ipAddress))
            {
                // Check if the request is already for the 403 error page
                if (context.Request.Path.Value.Equals("/Admin/Error/403", StringComparison.OrdinalIgnoreCase))
                {
                    // Do not redirect to avoid a loop
                    await _next(context);
                    return;
                }

                // Block access and redirect to the custom 403 page
                context.Response.Redirect("/Admin/Error/403");
                return; // Exit middleware
            }

            await _next(context);
        }
    }
}