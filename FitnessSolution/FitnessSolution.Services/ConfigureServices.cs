using FitnessSolution.Data.Providers.Implementations;
using FitnessSolution.Data.Providers.Interfaces;
using FitnessSolution.Services.Mapping;
using FitnessSolution.Services.Services.Implementations;
using FitnessSolution.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessSolution.Services
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITeamProvider, TeamProvider>();
            services.AddScoped<ICounterProvider, CounterProvider>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ICounterService, CounterService>();

            return services;
        }

        public static IServiceCollection AddServiceMapperConfiguration(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
