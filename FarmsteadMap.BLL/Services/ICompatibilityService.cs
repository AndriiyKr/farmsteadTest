using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface ICompatibilityService
    {
        Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId);
        Task<BaseResponseDTO<CompatibilityResultDTO>> CheckCompatibilityAsync(MapElementDTO element1, MapElementDTO element2);
    }
}