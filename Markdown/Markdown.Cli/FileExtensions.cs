using System;
using System.IO;
using System.Threading;

namespace Markdown.Cli
{
    public static class FileExtensions
    {
        public static void TryGetReadAccess(string filename)
        {
            using (File.OpenRead(filename))
            {
            }
        }

        public static void TryGetWriteAccess(string filename)
        {
            using (File.OpenWrite(filename))
            {
            }
        }

        public static bool HaveReadAccess(string filename)
        {
            try
            {
                TryGetReadAccess(filename);
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
                TryGetWriteAccess(filename);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}