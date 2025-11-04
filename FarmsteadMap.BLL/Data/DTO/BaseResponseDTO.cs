namespace FarmsteadMap.BLL.Data.DTO
{
    public class BaseResponseDTO<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }

        public static BaseResponseDTO<T> Ok(T data) => new() { Success = true, Data = data };
        public static BaseResponseDTO<T> Fail(string error) => new() { Success = false, Error = error };
    }
}