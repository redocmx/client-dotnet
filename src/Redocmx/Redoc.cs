
namespace Redocmx
{
    public class Redoc
    {
        private readonly string _apiKey;
        private readonly Service _service;

        public Redoc(string apiKey = null)
        {
            _apiKey = apiKey;
            _service = new Service(_apiKey);
        }

        public Cfdi Cfdi => new Cfdi(_service);
    }
}