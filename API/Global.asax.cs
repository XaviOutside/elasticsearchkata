using Autofac;
using Autofac.Integration.WebApi;
using ElasticSearch.Entities;
using ElasticSearch.Repositories;
using ElasticSearch.Services;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IContainer Container { get; set; }

        protected void Application_Start()
        {

            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            builder.RegisterType<ElasticSearchService>().As<IElasticSearchService>().InstancePerRequest();
            builder.RegisterType<ElasticSearchRepository>().As<IElasticSearchRepository>().InstancePerRequest();

            builder.RegisterType<ElasticIndexerService>().As<IElasticIndexerService<Gif>>().InstancePerRequest();
            builder.RegisterType<ElasticIndexerRepository>().As<IElasticIndexerRepository<Gif>>().InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
