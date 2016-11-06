using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Nodes
{
    public static class EmphasisExtensions
    {
        public static IEnumerable<EmphasisStrength> GetAllEmphasisStrengths()
        {
            return Enum.GetValues(typeof(EmphasisStrength)).Cast<EmphasisStrength>();
        }

        public static IEnumerable<EmphasisStrength> ExcludeFromAllStrengths(this EmphasisStrength strength)
        {
            return GetAllEmphasisStrengths().Except(new[] {strength});
        }
    }
}
