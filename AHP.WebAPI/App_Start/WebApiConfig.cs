using AHP.Model;
using AHP.WebAPI.Controllers;
using AHP.WebAPI.MapperProfiles;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

namespace AHP.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Registers all modules from referenced assemblies and all modules in WebAPI
            var builder = new ContainerBuilder();

            //var path = AppDomain.CurrentDomain.BaseDirectory;
            //var assemblies = Directory.GetFiles(path, "AHP.*.dll").Select(Assembly.LoadFrom).ToArray();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            Assembly[] assemblies = Directory.GetFiles(path, "AHP.*.dll").Select(Assembly.LoadFrom).ToArray();
           

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelMapperProfile());
                cfg.AddProfile(new ControllerMapperProfile());
            }));


            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            builder.RegisterAssemblyModules(assemblies.ToArray());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(typeof(SignUpController).Assembly);
            builder.RegisterControllers(typeof(LoggedInController).Assembly);


            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}