namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapShortDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Додали це поле, щоб малювати прев'ю
        public string MapJson { get; set; } = string.Empty;

        public bool IsPrivate { get; set; }
    }
}