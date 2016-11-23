using System.Collections.Generic;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlEmptyTag : IHtmlTag
    {
        public string OpeningTag => "";
        public string ClosingTag => "";
        public IEnumerable<HtmlAttribute> Attributes { get; }
        public IHtmlTag AddAttributes(IEnumerable<HtmlAttribute> attributes)
        {
            return this;
        }
    }
}