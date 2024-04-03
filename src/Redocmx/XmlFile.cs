using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Redocmx
{
    public class XmlFile
    {
        protected string _filePath;
        protected string _fileContent;

        public void FromFile(string filePath)
        {
            _filePath = filePath;
        }

        public void FromString(string fileContent)
        {
            _fileContent = fileContent;
        }

        public async Task<(string content, string type)> GetFile()
        {
            if (!string.IsNullOrEmpty(_fileContent))
            {
                return (_fileContent, "string");
            }

            if (!string.IsNullOrEmpty(_filePath))
            {
                if (!System.IO.File.Exists(_filePath))
                {
                    throw new FileNotFoundException($"Failed to read content from file: {_filePath}. The file does not exist.");
                }

                try
                {
                    string fileContent = await File.ReadAllTextAsync(_filePath, Encoding.UTF8);
                    _fileContent = fileContent;
                    return (_fileContent, "string");
                }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedAccessException($"Permission denied: {_filePath}. The file exists but cannot be read.");
                }

            }

            throw new InvalidOperationException($"Failed to load file {GetType().Name}, you must use FromFile or FromString.");
        }
    }
}

