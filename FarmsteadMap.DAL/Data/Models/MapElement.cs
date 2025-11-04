namespace FarmsteadMap.DAL.Data.Models
{
    public class MapElement
    {
        public string Type { get; set; }
        public long ElementId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Rotation { get; set; }
    }
}
