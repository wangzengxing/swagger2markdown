using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swagger.SwaggerToMarkdown.Markdown
{
    public class MarkdownDocument
    {
        private readonly StringBuilder _content;
        public MarkdownDocument()
        {
            _content = new StringBuilder();
        }

        public void Write(string content)
        {
            _content.Append(content);
        }

        public void WriteLine(string content)
        {
            _content.Append(content);
            _content.Append("\n");
        }

        public void WriteH1(string content)
        {
            _content.Append("# ");
            WriteLine(content);
        }

        public void WriteH2(string content)
        {
            _content.Append("## ");
            WriteLine(content);
        }

        public void WriteH3(string content)
        {
            _content.Append("### ");
            WriteLine(content);
        }

        public void WriteH4(string content)
        {
            _content.Append("#### ");
            WriteLine(content);
        }

        public void WriteH5(string content)
        {
            _content.Append("##### ");
            WriteLine(content);
        }

        public void WriteH6(string content)
        {
            _content.Append("###### ");
            WriteLine(content);
        }

        public void WriteUnorderedList(params string[] contents)
        {
            foreach (var content in contents)
            {
                _content.Append("- ");
                WriteLine(content);
            }
        }

        public void WriteOrderedList(params string[] contents)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                _content.Append($"{i + 1}. ");
                WriteLine(contents[i]);
            }
        }

        public void WriteTable(Action<MarkdownTableBuilder> builderAction)
        {
            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            var builder = new MarkdownTableBuilder(this);
            builderAction?.Invoke(builder);
        }

        public void WriteCode(string content)
        {
            WriteLine("```");
            WriteLine(content);
            WriteLine("```");
        }

        public override string ToString()
        {
            return _content.ToString();
        }
    }
}
