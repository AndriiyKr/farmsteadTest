namespace FarmsteadMap.DAL.Data.Models
{
    public class TreeSort
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroundType { get; set; }
        public long TreeId { get; set; }
        public Tree Tree { get; set; }
    }
}
