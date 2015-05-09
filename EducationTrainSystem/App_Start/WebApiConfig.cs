using System.Web.Http;

namespace EducationTrainSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "apiv1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
