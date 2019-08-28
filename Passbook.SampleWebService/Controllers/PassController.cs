using Microsoft.AspNetCore.Mvc;
using Passbook.SampleWebService.Models;
using Passbook.SampleWebService.Services;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Controllers
{
    public class PassController : Controller
    {
        private readonly IPassService _passService;

        public PassController(IPassService passService)
        {
            _passService = passService;
        }

        public IActionResult Index()
        {
            PassModel model = new PassModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PassModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate the pass.
                //
                var passContents = await _passService.GeneratePassAsync(model.SerialNumber, model.Value, model.Secret);

                // Return the file. You can download this onto your own iPhone.
                //
                return File(passContents, "pass/pass");
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