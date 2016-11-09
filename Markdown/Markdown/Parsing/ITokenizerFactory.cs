using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing
{
    public interface ITokenizerFactory<T> where T : class
    {
        ATokenizer<T> CreateTokenizer(string text);
    }
}
