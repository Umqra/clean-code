using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlTagsSequence : IHtmlTag
    {
        private List<IHtmlTag> Elements { get; }
        public string OpeningTag { get; private set; }
        public string ClosingTag { get; private set; }
        private List<HtmlAttribute> _attributes { get; }
        public IEnumerable<HtmlAttribute> Attributes => _attributes;
        public IHtmlTag AddAttributes(IEnumerable<HtmlAttribute> attributes)
        {
            var htmlAttributes = attributes.ToArray();
            _attributes.AddRange(htmlAttributes);
            foreach (var element in Elements)
                element.AddAttributes(htmlAttributes);
            InitTags(); 
            return this;
        }

        public HtmlTagsSequence(IEnumerable<IHtmlTag> elements)
        {
            Elements = elements.ToList();
            _attributes = new List<HtmlAttribute>();
            InitTags();
        }

        private void InitTags()
        {
            OpeningTag = string.Join("", Elements.Select(element => element.OpeningTag));
            ClosingTag = string.Join("", Enumerable.Reverse(Elements).Select(element => element.ClosingTag));
        }

        public HtmlTagsSequence(params IHtmlTag[] elements) : this((IEnumerable<IHtmlTag>)elements) { }
    }
}
