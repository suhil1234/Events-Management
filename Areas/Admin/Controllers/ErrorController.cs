using Microsoft.AspNetCore.Mvc;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorController : Controller
    {
        // Route for handling different error status codes
        [Route("Admin/Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404"); // Return 404 view
            }
            else if (statusCode == 403)
            {
                return View("403"); // Return 403 view
            }
            return View("500"); // Default to 500 error view
        }
    }
}