using Event_management.BL;
using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllersWithViews(); // Adds MVC services
builder.Services.AddDbContext<EventManagement2Context>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21)))); // Adjust version as necessary

// Register ClsAccessList for dependency injection
builder.Services.AddScoped<IAccessListService, ClsAccessList>();
// Register ClsSummary for dependency injection
builder.Services.AddScoped<ISummaryService,ClsSummary>();
// Register ClsEvents for dependency injection
builder.Services.AddScoped<IEventService, CLsEvents>();
// Register ClsLocations for dependency injection
builder.Services.AddScoped<ILocationService, ClsLocations>();
// Register ClsParticipants for dependency injection
builder.Services.AddScoped<IParticipantService, ClsParticipants>();


var app = builder.Build();

// Configure middleware for error handling in production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Admin/Error/500"); // Custom error handling for server errors
    app.UseHsts(); // Enable HTTP Strict Transport Security
}

// Middleware for IP blocking (uncomment to enable)
// app.UseMiddleware<IpBlockingMiddleware>();

// Middleware for handling 404 errors
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 404)
    {
        response.Redirect("/Admin/Error/404"); // Redirect to custom 404 page
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configure endpoint routing
app.MapControllerRoute(
    name: "default",
    pattern: "",
    defaults: new { area = "Admin", controller = "Home", action = "Index" }
);

// Route for other admin actions
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}"
);

// Route for error handling
app.MapControllerRoute(
    name: "error",
    pattern: "Admin/Error/{statusCode}", // Pattern for error status codes
    defaults: new { controller = "Error", action = "HandleError" }
);

// Run the application
app.Run();