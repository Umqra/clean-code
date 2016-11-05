using System;
using System.IO;

namespace Markdown.Cli
{
    public static class FileExtensions
    {
        public static bool HaveReadAccess(string filename)
        {
            try
            {
                using (File.OpenRead(filename))
                {
                }
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public static bool HaveWriteAccess(string filename)
        {
            try
            {
                using (File.OpenWrite(filename))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
