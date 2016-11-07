using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Nodes
{
    public static class EmphasisExtensions
    {
        public static EmphasisStrength[] GetAllEmphasisValues()
        {
            return Enum.GetValues(typeof(EmphasisStrength)).Cast<EmphasisStrength>().ToArray();
        }

        public static EmphasisStrength[] ExcludeFromEmphasisValues(this EmphasisStrength strength)
        {
            return GetAllEmphasisValues().Except(new[] {strength}).ToArray();
        }
    }
}
