using System;
using System.Linq;

namespace Markdown.Parsing.Tokens
{
    public static class TokenExtensions
    {
        public static bool HasAny(this IMdToken token, params Md[] attributes)
        {
            return attributes.Any(attribute => token.Has(attribute));
        }

        public static string UnexpectedTokenReason(this IMdToken token)
        {
            var attributesInfo = "";
            if (token is MdToken)
            {
                var mdToken = (MdToken)token;
                var attributeNames = mdToken.Attributes.Select(attr => Enum.GetName(typeof(Md), attr));
                attributesInfo = " with attributes: " + string.Join(", ", attributeNames);
            }
            return $"Unexpected token {token.Text}{attributesInfo}. May be you need to escape it.";
        }
    }
}