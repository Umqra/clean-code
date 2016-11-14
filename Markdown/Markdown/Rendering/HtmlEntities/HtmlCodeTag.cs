using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Nit: ^^^^^

namespace Markdown.Rendering.HtmlEntities
{
    class HtmlCodeTag : IHtmlTag
    {
        public string OpeningTag => "<pre>";
        public string ClosingTag => "</pre>";
    }
}
