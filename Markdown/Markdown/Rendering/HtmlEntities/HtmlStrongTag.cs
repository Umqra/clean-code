using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlStrongTag : IHtmlTag
    {
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";
    }
}
