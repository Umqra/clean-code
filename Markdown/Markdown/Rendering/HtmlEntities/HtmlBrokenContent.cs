using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Rendering.HtmlEntities
{
    class HtmlBrokenContent : IHtmlContent
    {
        public string Content { get; }
        public string FailReason { get; }

        public HtmlBrokenContent(string content, string failReason)
        {
            Content = content;
            FailReason = failReason;
        }
    }
}
