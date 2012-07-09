using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Passbook.Sample.Web
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      //api/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber
      routes.MapHttpRoute(
          name: "RegistraterPass",
          routeTemplate: "api/{version}/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}",
          defaults: new { Controller = "PassRegistration", Action = "Post" });

      //api/v1/devices/d754ba022bd37181fed36ead4e7cf5d8/registrations/pass.tomasmcguinness.com
      routes.MapHttpRoute(
          name: "FetchUpdatedPasses",
          routeTemplate: "api/{version}/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}",
          defaults: new { Controller = "PassRegistration", Action = "Get" }
      );

      //api/version/passes/passTypeIdentifier/serialNumber
      routes.MapHttpRoute(
          name: "FetchPass",
          routeTemplate: "api/{version}/passes/{passTypeIdentifier}/{serialNumber}",
          defaults: new { Controller = "PassRegistration", Action = "GetPass" }
      );

      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}