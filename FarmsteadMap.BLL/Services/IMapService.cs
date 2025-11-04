using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface IMapService
    {
        Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId);
        Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId);
        Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync();
        Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto);
        Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto);
        Task<BaseResponseDTO<bool>> DeleteMapAsync(long id);
    }
}