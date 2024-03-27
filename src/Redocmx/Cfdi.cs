using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Redocmx
{
    public class Cfdi
    {
        private Service _service;
        private string _addenda;
        private string _filePath;
        private string _fileContent;
        private Pdf _pdf;

        public Cfdi(Service service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public Cfdi FromFile(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            return this;
        }

        public Cfdi FromString(string fileContent)
        {
            _fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
            return this;
        }

        public async Task<(string Content, string Type)> GetXmlContentAsync()
        {
            if (!string.IsNullOrEmpty(_fileContent))
            {
                return (_fileContent, "string");
            }

            if (!string.IsNullOrEmpty(_filePath))
            {
                if (!File.Exists(_filePath))
                {
                    throw new FileNotFoundException($"Failed to read XML content from file: {this._filePath}. The file does not exist.");
                }

                try
                {
                    string fileContent = await File.ReadAllTextAsync(_filePath, Encoding.UTF8);
                    return (fileContent, "file");
                }
                catch (UnauthorizedAccessException)
                {
                    throw new UnauthorizedAccessException($"Permission denied:{_filePath}. The file exists but cannot be read.");
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException(ex.Message);
                }

            }

            throw new InvalidOperationException("XML content for the CFDI must be provided.");
        }

        public void SetAddenda(string addenda)
        {
            _addenda = addenda ?? throw new ArgumentNullException(nameof(addenda));
        }

        public string GetAddenda()
        {
            return _addenda;
        }

        public async Task<Pdf> ToPdfAsync(Dictionary<string, string> options = null)
        {
            if (_pdf != null)
            {
                return _pdf;
            }

            var (Content, Type) = await GetXmlContentAsync();

            if (!string.IsNullOrEmpty(GetAddenda()))
            {
                if (options == null) options = new Dictionary<string, string>();
                options["addenda"] = GetAddenda();
            }

            _pdf = await _service.ConvertCfdiAsync(Content, options);
            return _pdf;
        }
    }

}
