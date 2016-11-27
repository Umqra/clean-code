using System;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Markdown.Cli
{
    public class CliOptions
    {
        [YamlMember(Alias = "input")]
        public string InputFilename { get; set; }

        [YamlMember(Alias = "output")]
        public string OutputFilename { get; set; }

        [YamlMember(Alias = "base_url")]
        public string BaseUrl { get; set; }

        [YamlMember(Alias = "html_file")]
        public string HtmlFilename { get; set; }

        [YamlMember(Alias = "inject_element")]
        public string InjectedHtmlElement { get; set; }

        [YamlMember(Alias = "class")]
        public string InjectCssClass { get; set; }

        public string ConfigFilename { get; set; }

        public CliOptions TryInitialize()
        {
            TryInitializeConfigFile();

            TryInitializeInputFile();
            TryInitializeOutputFile();
            TryInitializeHtmlFile();
            return this;
        }

        private void TryInitializeConfigFile()
        {
            if (ConfigFilename == null)
                return;
            // CR (krait): Для этого написан экстеншн, но почему-то не используется.
            try
            {
                FileExtensions.TryGetReadAccess(ConfigFilename);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Can't read config file {ConfigFilename}", exception);
            }

            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new UnderscoredNamingConvention()).Build();

                CliOptions options;
                using (var reader = new StreamReader(File.OpenRead(ConfigFilename)))
                {
                    options = deserializer.Deserialize<CliOptions>(reader);
                }
                if (options == null)
                    throw new Exception("Can't retrieve data from config file");
                InputFilename = InputFilename ?? options.InputFilename;
                OutputFilename = OutputFilename ?? options.OutputFilename;
                BaseUrl = BaseUrl ?? options.BaseUrl;
                HtmlFilename = HtmlFilename ?? options.HtmlFilename;
                InjectedHtmlElement = InjectedHtmlElement ?? options.InjectedHtmlElement;
                InjectCssClass = InjectCssClass ?? options.InjectCssClass;
            }
            catch (Exception exception)
            {
                throw new YamlException($"Exception during parseing YAML configuartion file {ConfigFilename}", exception);
            }            
        }

        private void TryInitializeHtmlFile()
        {
            if (HtmlFilename != null)
            {
                if (InjectedHtmlElement == null)
                    throw new ArgumentException(
                        "InjectedField must be specified if used HtmlFilename. It can be specified with --inject_element option"
                    );
                try
                {
                    FileExtensions.TryGetReadAccess(HtmlFilename);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException($"Can't read from html file {HtmlFilename}", exception);
                }
            }
        }

        private void TryInitializeInputFile()
        {
            try
            {
                FileExtensions.TryGetReadAccess(InputFilename);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Can't read from input file {InputFilename}", exception);
            }
        }

        private void TryInitializeOutputFile()
        {
            try
            {
                FileExtensions.TryGetWriteAccess(InputFilename);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Can't write to output file {OutputFilename}", exception);
            }
        }
    }
}