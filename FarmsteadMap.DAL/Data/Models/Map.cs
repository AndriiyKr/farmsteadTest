namespace FarmsteadMap.DAL.Data.Models
{
    public class Map
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MapJson { get; set; }
        public bool IsPrivate { get; set; }
        public long UserId { get; set; }
    }
}
