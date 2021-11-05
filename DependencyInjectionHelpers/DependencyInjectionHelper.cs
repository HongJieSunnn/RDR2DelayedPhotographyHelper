using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace RDR2DelayedPhotographyHelper.DependencyInjectionHelpers
{
    public class DependencyInjectionHelper
    {
        private static bool Builded=false;
        private readonly static IServiceCollection _serivces=new ServiceCollection();
        
        public static IServiceProvider ServiceProvider{get;private set;}

        public static void BuildServiceProvider()
        {
            _serivces.AddSingleton<ILogger,Logger>((services)=>
            {
                return new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.WithProperty("ApplicationContext", "RDR2DelayedPhotographyHelper")
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
            });
            if(!Builded)
            {
                Builded=true;
                ServiceProvider=_serivces.BuildServiceProvider();
            }
        }
    }
}