using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class CreateMapDTO
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public long UserId { get; set; }
        public MapDataDTO MapData { get; set; }
    }
}
