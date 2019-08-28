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
                return new FileContentResult();
            }

            return View(model);
        }
        public IActionResult Update()
        {
            return View();
        }

        public IActionResult Update(UpdatePassModel model)
        {
            if(ModelState.IsValid)
            {

            }

            return View(model);
        }
    }
}