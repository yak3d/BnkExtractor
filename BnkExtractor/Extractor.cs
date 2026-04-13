using BnkExtractor.Ww2ogg;
using System.IO;
using System.Reflection;

namespace BnkExtractor
{
    public class Extractor
    {
        private static string _codebooksTempPath;

        private static string GetCodebooksPath()
        {
            if (_codebooksTempPath != null && File.Exists(_codebooksTempPath))
                return _codebooksTempPath;

            const string resourceName = "BnkExtractor.Ww2ogg.Codebooks.packed_codebooks_aoTuV_603.bin";
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");

            _codebooksTempPath = Path.Combine(Path.GetTempPath(), "packed_codebooks_aoTuV_603.bin");
            using var fs = File.Create(_codebooksTempPath);
            stream.CopyTo(fs);

            return _codebooksTempPath;
        }

        public static void ParseBnk(string filePath) => BnkExtr.BnkParser.Parse(filePath, false, false, false);
        public static void RevorbOgg(string inputFilePath) => Revorb.RevorbSharp.Convert(inputFilePath, null);
        public static void ConvertWem(string filePath)
        {
            Ww2oggOptions options = new Ww2oggOptions();
            options.InFilename = filePath;
            options.OutFilename = Path.ChangeExtension(filePath, "ogg");
            options.CodebooksFilename = GetCodebooksPath();
            Ww2oggConverter.Main(options);
        }
    }
}
