namespace FarmsteadMap.DAL.Data.Models
{
    public class VegSort
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string GroundType { get; set; }
        public long VegId { get; set; }
        public Vegetable Vegetable { get; set; }
    }
}
