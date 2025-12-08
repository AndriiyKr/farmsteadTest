namespace FarmsteadMap.BLL.Data.DTO
{
    public class UpdateMapDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? MapJson { get; set; } // Використовуємо MapJson
        public bool? IsPrivate { get; set; }
    }
}