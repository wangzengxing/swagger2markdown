using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Swagger.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerToMarkdown(this IApplicationBuilder builder, Action<SwaggerUIOptions> optionsAction = null)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");
                optionsAction?.Invoke(options);
            });

            return builder.UseMiddleware<SwaggerToMarkdownMiddleware>();
        }
    }
}
