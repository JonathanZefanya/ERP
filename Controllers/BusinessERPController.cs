using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class BusinessERPController : Controller
    {
        public BusinessERPController()
        {

        }

        [Authorize(Roles = Pages.MainMenu.BusinessERP.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
