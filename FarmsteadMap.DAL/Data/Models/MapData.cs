namespace FarmsteadMap.DAL.Data.Models
{
    public class MapData
    {
        public string Name { get; set; }
        public List<MapElement> Elements { get; set; } = new();
        public List<MeasurementLine> Measurements { get; set; } = new();
        public List<GardenBed> GardenBeds { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
