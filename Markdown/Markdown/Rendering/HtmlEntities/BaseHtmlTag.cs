using System.Collections.Generic;
using System.Linq;

namespace Markdown.Rendering.HtmlEntities
{
    public class BaseHtmlTag : IHtmlTag
    {
        private string Tag { get; }
        private List<HtmlAttribute> Attributes { get; }

        public string OpeningTag
        {
            get
            {
                if (Attributes.Count == 0)
                    return $"<{Tag}>";
                return $"<{Tag} {string.Join(" ", Attributes.Select(attribute => attribute.ToString()))}>";
            }
        }

        public string ClosingTag => $"</{Tag}>";

        IEnumerable<HtmlAttribute> IHtmlTag.Attributes => Attributes;

        public BaseHtmlTag(string tag, params HtmlAttribute[] attributes)
        {
            Attributes = attributes.ToList();
            Tag = tag;
        }

        public IHtmlTag AddAttributes(IEnumerable<HtmlAttribute> attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }
    }
}