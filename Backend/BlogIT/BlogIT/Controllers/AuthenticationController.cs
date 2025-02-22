using Microsoft.AspNetCore.Mvc;

namespace BlogIT.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
