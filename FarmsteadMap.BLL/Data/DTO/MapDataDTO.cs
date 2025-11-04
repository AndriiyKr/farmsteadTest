using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapDataDTO
    {
        public string Name { get; set; }
        public List<MapElementDTO> Elements { get; set; } = new();
        public List<MeasurementLineDTO> Measurements { get; set; } = new();
        public List<GardenBedDTO> GardenBeds { get; set; } = new();
    }
}
