using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhereAreMyStaff.Services.Mobile;

namespace Passbook.Sample.Web.Controllers
{
    public class UpdateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string deviceIdentifier)
        {
            iPhoneMobileNotificationService service = new iPhoneMobileNotificationService();
            service.SendEmptyPushNotification(deviceIdentifier, "46c1e98690ab593f2303f916c37668fdaa327a76");// ‎"46c1e98690ab593f2303f916c37668fdaa327a76");
            return View();
        }
    }
}
