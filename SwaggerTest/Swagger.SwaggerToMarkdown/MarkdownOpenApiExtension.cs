using Microsoft.OpenApi;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace Swagger.SwaggerToMarkdown
{
    public class MarkdownOpenApiExtension : IOpenApiExtension
    {
        private readonly Type _responseType;

        public MarkdownOpenApiExtension(Type responseType)
        {
            _responseType = responseType;
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteRaw(ToString());
        }

        public override string ToString()
        {
            if (_responseType == null)
            {
                return string.Empty;
            }

            var serializer = new Newtonsoft.Json.JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (typeof(IEnumerable).IsAssignableFrom(_responseType))
            {
                var jArray = JArray.FromObject(Activator.CreateInstance(_responseType), serializer);

                if (_responseType.GenericTypeArguments.Length > 0)
                {
                    jArray.Add(JObject.FromObject(Activator.CreateInstance(_responseType.GenericTypeArguments.FirstOrDefault()), serializer));
                }

                return jArray.ToString();
            }

            return JObject.FromObject(Activator.CreateInstance(_responseType), serializer).ToString();
        }
    }
}
