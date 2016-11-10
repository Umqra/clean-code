using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface INodeConverter<out T>
    {
        T Convert(INode node);
    }
}
