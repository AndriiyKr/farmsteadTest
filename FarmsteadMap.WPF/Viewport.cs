namespace FarmsteadMap.WPF
{
    using Newtonsoft.Json;

    public class Viewport
    {
        [JsonProperty("zoom")]
        public double Zoom { get; set; } = 1.0;

        [JsonProperty("offsetX")]
        public double OffsetX { get; set; }

        [JsonProperty("offsetY")]
        public double OffsetY { get; set; }
    }
}