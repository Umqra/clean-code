using System;
using System.Linq;

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