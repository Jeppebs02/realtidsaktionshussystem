using System.Text.Json.Serialization.Metadata;

namespace AuctionHouse.Requester
{
    internal class JsonSerializerOptions : JsonTypeInfo
    {
        public object PropertyNamingPolicy { get; set; }
        public bool WriteIndented { get; set; }
        public object DefaultIgnoreCondition { get; set; }
    }
}