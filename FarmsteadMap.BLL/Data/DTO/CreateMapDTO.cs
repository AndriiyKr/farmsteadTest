namespace FarmsteadMap.BLL.Data.DTO
{
    public class CreateMapDTO
    {
        required public string Name { get; set; }
        required public bool IsPrivate { get; set; }
        required public long UserId { get; set; }

        // Змінено з MapDataDTO на string, щоб передавати JSON
        public string? MapJson { get; set; }
    }
}