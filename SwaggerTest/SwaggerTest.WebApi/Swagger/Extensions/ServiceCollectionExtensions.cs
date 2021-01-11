using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Swagger.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerToMarkdown(this IServiceCollection services, Action<SwaggerToMarkdownOptions> builderAction, Action<SwaggerGenOptions> optionsAction = null)
        {
            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<MarkdownOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                optionsAction?.Invoke(options);
            });
            services.Configure(builderAction);

            return services;
        }
    }
}
