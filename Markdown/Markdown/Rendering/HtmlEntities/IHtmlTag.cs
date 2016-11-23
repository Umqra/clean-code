using System.Collections.Generic;

namespace Markdown.Rendering.HtmlEntities
{
    public interface IHtmlTag
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
        IEnumerable<HtmlAttribute> Attributes { get; }

        IHtmlTag AddAttributes(IEnumerable<HtmlAttribute> attributes);
    }
}