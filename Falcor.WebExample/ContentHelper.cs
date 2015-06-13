using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Kiwi.Markdown;
using Kiwi.Markdown.ContentProviders;

namespace Falcor.WebExample
{
    public static class ContentHelper
    {
        public static string GetIndex()
        {
            var index = File.ReadAllText(HostingEnvironment.MapPath("~/index.html"));
            return index.Replace("(readme.md)", GetReadme());
        }

        public static string GetReadme()
        {
            var service = new MarkdownService(new FileContentProvider(HostingEnvironment.MapPath("~/App_Data")));
            var readme = File.ReadAllText(HostingEnvironment.MapPath("~/readme.md"));
            readme = readme.Replace("[Model](Model.cs)", GetFileContentAsMarkdown("Model.cs"));
            return service.ToHtml(readme);
        }

        private static string GetFileContentAsMarkdown(string file)
        {
            var content = File.ReadAllText(HostingEnvironment.MapPath("~/" + file));
            var result = new StringBuilder();
            result.AppendLine("```CSharp");
            result.AppendLine(content);
            result.AppendLine("```");
            return result.ToString();
        }
    }
}