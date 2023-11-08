using CustomLibrary.Filters;
using CustomLibrary.Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomLibrary.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetEntryAssembly();
            TypeAdapterConfig.GlobalSettings.Scan(thisAssembly);
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

            return services;
        }
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(ModelValidationFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddCors();

            return services;
        }

    }
}
