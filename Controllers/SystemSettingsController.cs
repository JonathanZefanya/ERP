using BusinessERP.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SystemSettingsController : Controller
    {

        public SystemSettingsController()
        {
        }

        [Authorize(Roles = MainMenu.Admin.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

