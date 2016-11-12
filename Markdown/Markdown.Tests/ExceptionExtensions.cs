using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tests
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> EnumerateInnerExceptions(this Exception exception)
        {
            while (exception != null)
            {
                yield return exception;
                exception = exception.InnerException;
            }
        }
    }
}
