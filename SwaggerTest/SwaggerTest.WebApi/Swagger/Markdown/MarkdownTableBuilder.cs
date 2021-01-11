using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerTest.WebApi.Swagger.Markdown
{
    public class MarkdownTableBuilder
    {
        private readonly MarkdownDocument _document;

        public MarkdownTableBuilder(MarkdownDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public MarkdownTableBuilder AddHeader(params string[] columns)
        {
            foreach (var column in columns)
            {
                _document.Write("|");
                _document.Write(column);
            }
            _document.WriteLine("|");

            for (int i = 0; i < columns.Length; i++)
            {
                _document.Write("|:-----");
            }
            _document.WriteLine("|");

            return this;
        }

        public MarkdownTableBuilder AddRow(params string[] columns)
        {
            foreach (var column in columns)
            {
                _document.Write("|");
                _document.Write(column);
                _document.Write(" ");
            }
            _document.WriteLine("|");

            return this;
        }
    }
}
