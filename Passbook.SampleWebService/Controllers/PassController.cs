using Microsoft.AspNetCore.Mvc;
using Passbook.SampleWebService.Models;
using Passbook.SampleWebService.Services;
using System;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Controllers
{
    public class PassController : Controller
    {
        private readonly IPassService _passService;
        private readonly PassGeneratorConfiguration _configuration;

        public PassController(IPassService passService, PassGeneratorConfiguration configuration)
        {
            _passService = passService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (_configuration.IsValid())
            {
                return View();
            }
            else
            {
                return RedirectToAction("CheckConfiguration");
            }
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

        public IActionResult CheckConfiguration()
        {
            return View();
        }
    }
}