using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redocmx
{
    public class Cfdi : XmlFile
    {
        private Service _service;
        private Addenda _addenda;
        private Dictionary<string, string> _addendaReplaceValues;
        private Pdf _pdf;

        public Cfdi() : base()
        {
            _service = Service.GetInstance();
        }

        public new Cfdi FromFile(string filePath)
        {
            this._filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            return this;
        }

        public new Cfdi FromString(string fileContent)
        {
            this._fileContent = fileContent ?? throw new ArgumentNullException(nameof(fileContent));
            return this;
        }

        public void SetAddenda(Addenda addenda, Dictionary<string, string> replaceValues = null)
        {
            _addenda = addenda;
            _addendaReplaceValues = replaceValues;
        }

        public async Task<Pdf> ToPdfAsync(Dictionary<string, string> options = null)
        {
            if (_pdf != null)
            {
                return _pdf;
            }

            var (Content, Type) = await this.GetFile();

            if (_addenda != null)
            {
                options ??= new Dictionary<string, string>();

                string addendaContent = await _addenda.GetFileContent(this._addendaReplaceValues);
                options["addenda"] = addendaContent;
            }

            _pdf = await _service.ConvertCfdiAsync(Content, options);
            return _pdf;
        }
    }

}
