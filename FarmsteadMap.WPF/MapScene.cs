namespace FarmsteadMap.WPF
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MapScene
    {
        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; } = new Viewport();

        [JsonProperty("objects")]
        public List<MapObject> Objects { get; set; } = new List<MapObject>();

        [JsonProperty("landscape")]
        public List<LandscapeArea> Landscape { get; set; } = new List<LandscapeArea>();
    }
}