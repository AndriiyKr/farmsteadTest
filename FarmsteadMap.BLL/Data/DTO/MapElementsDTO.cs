using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapElementsDTO
    {
        public List<TreeSortDTO> TreeSorts { get; set; } = new();
        public List<VegSortDTO> VegSorts { get; set; } = new();
        public List<FlowerDTO> Flowers { get; set; } = new();
    }
}
