namespace FarmsteadMap.BLL.Data.DTO
{
    using System.Collections.Generic;

    public class MapElementsDTO
    {
        public List<BasePlantDTO> Trees { get; set; } = new();
        public List<BasePlantDTO> Vegetables { get; set; } = new();
        public List<FlowerDTO> Flowers { get; set; } = new(); // FlowerDTO вже є, або використовуйте BasePlantDTO

        // Правила несумісності (зберігаємо пари ID)
        public List<IncompatibilityDTO> TreeIncompatibilities { get; set; } = new();
        public List<IncompatibilityDTO> VegIncompatibilities { get; set; } = new();
    }
}