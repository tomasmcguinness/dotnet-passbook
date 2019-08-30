using Microsoft.AspNetCore.Mvc;
using Passbook.SampleWebService.Models;
using Passbook.SampleWebService.Services;
using System;
using System.Security.Cryptography.X509Certificates;
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
            var model = new PassModel();
            model.HasInvalidConfiguration = !_configuration.IsValid();
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

            model.HasInvalidConfiguration = !_configuration.IsValid();

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
            var model = new ConfigurationModel();

            if (string.IsNullOrEmpty(_configuration.AppleWWDRCACertificatePath))
            {
                model.AppleWWDRCACertificatePathMessage = "You must provide a value for this configuration item.";
            }
            else
            {
                if (System.IO.File.Exists(_configuration.AppleWWDRCACertificatePath))
                {
                    try
                    {
                        var cert = new X509Certificate(_configuration.AppleWWDRCACertificatePath);
                    }
                    catch
                    {
                        model.AppleWWDRCACertificatePathMessage = $"The file specified at \"{_configuration.AppleWWDRCACertificatePath}\" does not appear to be a valid certificate.";
                    }
                }
                else
                {
                    model.AppleWWDRCACertificatePathMessage = $"The file specified at \"{_configuration.AppleWWDRCACertificatePath}\" could not be found.";
                }
            }

            if (string.IsNullOrEmpty(_configuration.PassTypeCertificatePassword))
            {
                model.WebServiceUrlMessage = "You must provide a password for certificate.";
            }

            if (string.IsNullOrEmpty(_configuration.PassTypeCertificatePath))
            {
                model.CertificatePathMessage = "You must provide a value for this configuration item.";
            }
            else
            {
                if (System.IO.File.Exists(_configuration.PassTypeCertificatePath))
                {
                    if (string.IsNullOrEmpty(_configuration.PassTypeCertificatePassword))
                    {
                        model.CertificatePathMessage = "You must provide a password so the certificate can be loaded.";
                    }
                    else
                    {
                        try
                        {
                            var cert = new X509Certificate(_configuration.PassTypeCertificatePath, _configuration.PassTypeCertificatePassword);
                        }
                        catch
                        {
                            model.CertificatePathMessage = $"The file specified at \"{_configuration.AppleWWDRCACertificatePath}\" does not appear to be a valid certificate.";
                        }
                    }
                }
                else
                {
                    model.CertificatePathMessage = $"The file specified at \"{_configuration.AppleWWDRCACertificatePath}\" could not be found.";
                }
            }

            if (string.IsNullOrEmpty(_configuration.WebServiceUrl))
            {
                model.WebServiceUrlMessage = "You must provide a value for this configuration item.";
            }
            else
            {
                if (_configuration.WebServiceUrl.StartsWith("http://"))
                {
                    model.WebServiceUrlMessage = "This is a HTTP url. Please ensure you enable support for HTTP in your device's settings.";
                }
            }

            if (string.IsNullOrEmpty(_configuration.PassTypeIdentifier))
            {
                model.PassTypeIdentifierMessage = "You must provide a value for this configuration item.";
            }

            if (string.IsNullOrEmpty(_configuration.TeamIdentifier))
            {
                model.TeamIdentifierMessage = "You must provide a value for this configuration item.";
            }

            return View(model);
        }
    }
}