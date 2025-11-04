using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class UpdateMapDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public MapDataDTO MapData { get; set; }
    }

}
