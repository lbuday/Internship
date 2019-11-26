using AHP.DAL;
using AHP.Repository.Common;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Repository
{
    class RepositoryModule : Autofac.Module
    {   //Register components in Load()
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();

            builder.RegisterType<AHPEntities>().AsSelf();

        }
    }
}
