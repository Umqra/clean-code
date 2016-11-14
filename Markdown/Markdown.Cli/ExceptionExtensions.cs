using System;
using System.Collections.Generic;

namespace Markdown.Cli
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