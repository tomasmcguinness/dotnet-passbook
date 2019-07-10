using Microsoft.AspNetCore.Mvc;

namespace Passbook.SampleWebService.Controllers
{
    public class PassController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }
    }
}