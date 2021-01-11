﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SwaggerTest.WebApi.Swagger.Extensions;
using SwaggerTest.WebApi.Swagger.Markdown;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Swagger
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SwaggerToMarkdownMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly SwaggerToMarkdownOptions _options;

        public SwaggerToMarkdownMiddleware(RequestDelegate next, ISwaggerProvider swaggerProvider, IOptions<SwaggerToMarkdownOptions> optionsAccesser)
        {
            _next = next;
            _swaggerProvider = swaggerProvider;
            _options = optionsAccesser.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(_options.RelativePath))
            {
                var doc = new MarkdownDocument();

                var swagger = _swaggerProvider.GetSwagger(_options.DocumentName);
                foreach (var path in swagger.Paths)
                {
                    foreach (var operation in path.Value.Operations)
                    {
                        doc.WriteH4("简要描述");
                        doc.WriteUnorderedList(operation.Value.Summary);

                        doc.WriteH4("请求URL");
                        doc.WriteUnorderedList(path.Key);

                        doc.WriteH4("请求方式");
                        doc.WriteUnorderedList(operation.Key.ToString());

                        doc.WriteH4("参数");
                        doc.WriteTable(builder =>
                        {
                            if (operation.Value.Parameters.Count > 0)
                            {
                                builder.AddHeader("参数名", "必选", "类型", "说明");

                                foreach (var parameter in operation.Value.Parameters)
                                {
                                    builder.AddRow(parameter.Name.ToTitleCase(), parameter.Required ? "是" : "否", parameter.Schema.Type, parameter.Description);
                                }
                            }
                            else
                            {
                                if (operation.Value.RequestBody != null)
                                {
                                    var openApiMediaType = operation.Value.RequestBody.Content["application/json"];
                                    var reference = openApiMediaType.Schema.Reference.ReferenceV3;

                                    var openApiSchema = GetSchema(swagger, reference);
                                    if (openApiSchema.Properties.Count > 0)
                                    {
                                        builder.AddHeader("参数名", "必选", "类型", "说明");

                                        foreach (var property in openApiSchema.Properties)
                                        {
                                            builder.AddRow(property.Key.ToTitleCase(), "否", property.Value.Type, property.Value.Description);
                                        }
                                    }
                                }
                            }
                        });

                        doc.WriteH4("返回示例");
                        var responseTypeJson = operation.Value.Extensions["ResponseType"].ToString();
                        doc.WriteCode(responseTypeJson);

                        doc.WriteH4("返回参数说明");
                        doc.WriteTable(builder =>
                        {
                            if (operation.Value.Responses.TryGetValue("200", out var openApiResponse))
                            {
                                if (openApiResponse.Content.TryGetValue("application/json", out var openApiMediaType))
                                {
                                    var openApiSchema = GetSchema(swagger, openApiMediaType.Schema.Reference.ReferenceV3);
                                    if (openApiSchema.Properties.Count > 0)
                                    {
                                        builder.AddHeader("参数名", "类型", "说明");

                                        var prefix = string.Empty;
                                        AddProperties(builder, openApiSchema.Properties, ref prefix, swagger);
                                    }
                                }
                            }
                        });
                    }
                }

                httpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=swaggerToMarkdown.md; filename*=UTF-8''swaggerToMarkdown.md");
                httpContext.Response.ContentType = "text/markdown";
                await httpContext.Response.WriteAsync(doc.ToString());
            }
            else
            {
                await _next(httpContext);
            }
        }

        private static OpenApiSchema GetSchema(OpenApiDocument swagger, string reference)
        {
            var schema = reference.Substring(reference.LastIndexOf("/") + 1);
            var openApiSchema = swagger.Components.Schemas[schema];

            return openApiSchema;
        }

        private static void AddProperties(MarkdownTableBuilder builder, IDictionary<string, OpenApiSchema> properties, ref string prefix, OpenApiDocument swagger)
        {
            foreach (var property in properties)
            {
                builder.AddRow(prefix + property.Key.ToTitleCase(), property.Value.Type, property.Value.Description);

                var reference = property.Value.Reference?.ReferenceV3;
                if (property.Value.Type == "array")
                {
                    reference = property.Value.Items.Reference?.ReferenceV3;
                }

                if (reference != null)
                {
                    var openApiSchema = GetSchema(swagger, reference);
                    if (openApiSchema.Properties.Count > 0)
                    {
                        prefix += property.Key.ToTitleCase() + ".";
                        AddProperties(builder, openApiSchema.Properties, ref prefix, swagger);
                    }
                }
            }
        }
    }
}
