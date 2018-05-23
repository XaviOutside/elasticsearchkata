using Extensions.Logger;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            "--------------- START PROGRAM ---------------".ToLog();

            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "Create Index",
                 routeTemplate: "elastic/createindex",
                defaults: new { action = "CreateIndex", controller = "Elastic" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
             );

            //config.Routes.MapHttpRoute(
            //    name: "Create Index",
            //    routeTemplate: "api/elastic/createindex",
            //    defaults: new { action = "CreateIndex", controller = "Elastic" }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
