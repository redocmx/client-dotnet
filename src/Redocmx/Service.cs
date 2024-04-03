using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Redocmx
{
    public class Service
    {
        private static Service _instance;
        private static readonly object _lock = new object();

        private readonly string _apiKey;
        private readonly string _apiUrl;

        public Service(string apiKey)
        {
            _apiKey = apiKey ?? Environment.GetEnvironmentVariable("REDOC_API_KEY");
            _apiUrl = Environment.GetEnvironmentVariable("REDOC_API_URL") ?? "https://api.redoc.mx/cfdis/convert";
            _instance = this;
        }

        public static Service GetInstance(string apiKey = null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Service(apiKey);
                }
                return _instance;
            }
        }

        public async Task<Pdf> ConvertCfdiAsync(string xmlContent, Dictionary<string, string> options = null)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
            client.DefaultRequestHeaders.Add("X-Redoc-Api-Key", _apiKey);

            var content = new MultipartFormDataContent();

            var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(xmlContent));
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            content.Add(byteArrayContent, "xml", "document.xml");

            if (options != null)
            {
                foreach (var option in options)
                {
                    content.Add(new StringContent(option.Value, Encoding.UTF8), option.Key);
                }
            }

            var response = await client.PostAsync(_apiUrl, content);
            response.EnsureSuccessStatusCode();

            var pdfContent = await response.Content.ReadAsByteArrayAsync();
            var transactionId = response.Headers.GetValues("x-redoc-transaction-id").FirstOrDefault();
            var totalPages = int.Parse(response.Headers.GetValues("x-redoc-pdf-total-pages").FirstOrDefault());
            var totalTime = int.Parse(response.Headers.GetValues("x-redoc-process-total-time").FirstOrDefault());

            var metadataBase64 = response.Headers.GetValues("x-redoc-xml-metadata").FirstOrDefault();

            var jsonBytes = Convert.FromBase64String(metadataBase64);
            var jsonString = Encoding.UTF8.GetString(jsonBytes);

            var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

            return new Pdf(pdfContent, transactionId, totalPages, totalTime, metadata);
        }
    }
}
