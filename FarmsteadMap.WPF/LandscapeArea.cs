namespace FarmsteadMap.WPF
{
    using System.Collections.Generic;
    using System.Windows;
    using Newtonsoft.Json;

    public class LandscapeArea
    {
        [JsonProperty("areaType")]
        public string AreaType { get; set; } = string.Empty;

        // Distinguish between Polygon (false) and Polyline (true)
        [JsonProperty("isPath")]
        public bool IsPath { get; set; }

        // Store metadata like Building Name and Height
        [JsonProperty("properties")]
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        [JsonProperty("points")]
        public List<Point> Points { get; set; } = new List<Point>();
    }
}