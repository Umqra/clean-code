using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlLinkTag : IHtmlTag
    {
        public string OpeningTag => $"<a href=\"{Reference}\">";
        public string ClosingTag => "</a>";
        public string Reference { get; }

        public HtmlLinkTag(string reference)
        {
            Reference = reference;
        }
    }
}
