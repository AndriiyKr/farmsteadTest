namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MapJson { get; set; }
        public bool IsPrivate { get; set; }
        public long UserId { get; set; }

    }
}
