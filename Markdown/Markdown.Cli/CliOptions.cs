using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Markdown.Cli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public string BaseUrl { get; set; }
        public string HtmlFilename { get; set; }
        public string InjectedHtmlElement { get; set; }
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
                var options = deserializer.Deserialize<CliOptions>(new StreamReader(File.OpenRead(ConfigFilename)));
                InputFilename = InputFilename ?? options.InputFilename;
                OutputFilename = OutputFilename ?? options.OutputFilename;
                BaseUrl = BaseUrl ?? options.BaseUrl;
                HtmlFilename = HtmlFilename ?? options.HtmlFilename;
                InjectedHtmlElement = InjectedHtmlElement ?? options.InjectedHtmlElement;
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
                        "InjectedField must be specified if used HtmlFilename. It can be specified with --inject_el option"
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