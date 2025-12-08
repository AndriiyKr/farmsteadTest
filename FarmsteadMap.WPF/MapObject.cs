namespace FarmsteadMap.WPF
{
    using Newtonsoft.Json;

    public class MapObject
    {
        [JsonProperty("dbId")]
        public long DbId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }
}