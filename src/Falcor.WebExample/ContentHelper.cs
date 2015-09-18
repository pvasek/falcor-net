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
            var index = File.ReadAllText(HostingEnvironment.MapPath("~/docroot.html"));
            return index.Replace("(readme.md)", GetReadme());
        }

        private static string GetReadme()
        {
            var service = new MarkdownService(new FileContentProvider(HostingEnvironment.MapPath("~/App_Data")));
            var readme = File.ReadAllText(HostingEnvironment.MapPath("~/readme.md"));
            var html = service.ToHtml(readme);
            html = html.Replace("http://falcor-net.azurewebsites.net/model.json?paths", "/model.json?paths");
            return html;
        }
    }
}