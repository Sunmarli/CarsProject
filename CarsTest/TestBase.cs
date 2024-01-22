﻿using CarsProject.AplicationServices;
using CarsProject.Core.ServiceInterface;
using CarsProject.Data;
using CarsTest.Macros;
using CarsTest.Mock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsTest
{
    public abstract class TestBase
    {
        protected IServiceProvider serviceProvider { get; }

        protected TestBase()
        {

            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();


        }
        public void Dispose()
        {

        }
        protected T Svc<T>()
        {
            return serviceProvider.GetService<T>();

        }

        protected T Macro<T>() where T : IMacros
        {
            return serviceProvider.GetService<T>();
        }


        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<ICarsServices, CarsServices>();
            
            services.AddScoped<IHostEnvironment, MockHostEnvironment>();

            services.AddDbContext<CarsProjectContext>(x =>
            {
                x.UseInMemoryDatabase("TEST");
                x.ConfigureWarnings(e => e.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }
        private void RegisterMacros(IServiceCollection services)
        {
            var macrosBaseType = typeof(IMacros);


            var macros = macrosBaseType.Assembly.GetTypes()
                .Where(x => macrosBaseType.IsAssignableFrom(x) && !x.IsInterface
                && !x.IsAbstract);

            foreach (var macro in macros)
            {
                services.AddSingleton(macro);

            }


        }

    }
}
