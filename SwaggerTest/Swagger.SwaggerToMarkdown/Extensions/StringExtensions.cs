using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger.SwaggerToMarkdown.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string text)
        {
            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }
    }
}
