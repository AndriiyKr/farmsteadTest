namespace FarmsteadMap.BLL.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    public interface IMapService
    {
        Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId);
        Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId);
        Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync();
        Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto);
        Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto);
        Task<BaseResponseDTO<bool>> DeleteMapAsync(long id);

        // Нові методи для сортів
        Task<BaseResponseDTO<List<SortDTO>>> GetTreeSortsAsync(long treeId);
        Task<BaseResponseDTO<List<SortDTO>>> GetVegSortsAsync(long vegId);
    }
}