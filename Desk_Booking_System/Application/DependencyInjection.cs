using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) // IServiceCollection
        {
            services.AddMediatR(configuration => // wires the query handling and configuration
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); // this confidures MediaTr from assmebly;
            });

            return services;
        }
    }
}
