using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlEmptyTag : IHtmlTag
    {
        public string OpeningTag => "";
        public string ClosingTag => "";
    }
}
