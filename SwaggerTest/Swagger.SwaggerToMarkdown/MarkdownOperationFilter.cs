using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger.SwaggerToMarkdown
{
    public class MarkdownOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Extensions.Add("ResponseType", new MarkdownOpenApiExtension(context.ApiDescription.SupportedResponseTypes.FirstOrDefault()?.Type));
        }
    }
}
