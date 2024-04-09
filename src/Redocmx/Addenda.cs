using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redocmx
{
    public class Addenda : XmlFile
    {
        public Addenda() : base() {}

        public new Addenda FromFile(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            return this;
        }

        public new Addenda FromString(string fileContent)
        {
            _fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
            return this;
        }

        public string ReplaceValues(string content, Dictionary<string, string> options = null)
        {
            if (options == null)
                return content;

            foreach (var option in options)
            {
                var key = option.Key;
                var value = option.Value;
                content = content.Replace(key, value);
            }

            return content;
        }

        public async Task<string> GetFileContent(Dictionary<string, string> replaceValues)
        {
            var file = await GetFile();
            var fileContent = file.content.ToString();

            return ReplaceValues(fileContent, replaceValues);
        }
    }
}
