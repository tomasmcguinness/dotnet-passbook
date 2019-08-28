using Microsoft.AspNetCore.Mvc;
using Passbook.SampleWebService.Models;

namespace Passbook.SampleWebService.Controllers
{
    public class PassController : Controller
    {
        public IActionResult Index()
        {
            PassModel model = new PassModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PassModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate the pass.
                //

                // Return the file. You can download this onto your own iPhone.
                //
                return File(new byte[0], "pass/pass");
            }

            return View(model);
        }
        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Update(UpdatePassModel model)
        {
            if (ModelState.IsValid)
            {

            }

            return View(model);
        }
    }
}