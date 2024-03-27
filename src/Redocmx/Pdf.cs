
using System.Collections.Generic;

namespace Redocmx
{
    public class Pdf
    {
        public byte[] Buffer { get; private set; }
        public string TransactionId { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalTimeMs { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }

        public Pdf(byte[] buffer, string transactionId, int totalPages, int totalTimeMs, Dictionary<string, object> metadata)
        {
            Buffer = buffer;
            TransactionId = transactionId;
            TotalPages = totalPages;
            TotalTimeMs = totalTimeMs;
            Metadata = metadata;
        }
    }
}