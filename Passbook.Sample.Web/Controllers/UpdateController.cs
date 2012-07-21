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
            service.SendEmptyPushNotification(deviceIdentifier, "B21B3785CBA2360A7B51635B82A4B4F5C3534AFA");
            return View();
        }
    }
}
