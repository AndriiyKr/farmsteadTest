using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface IMapToolsService
    {
        MapElementDTO AddTreeElement(long treeSortId, double x, double y);
        MapElementDTO AddVegetableElement(long vegSortId, double x, double y);
        MapElementDTO AddFlowerElement(long flowerId, double x, double y);
        MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY);
        GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape);

        Task<BaseResponseDTO<MapElementDTO>> AddTreeElementWithValidation(long treeSortId, double x, double y, List<MapElementDTO> existingElements);
        Task<BaseResponseDTO<MapElementDTO>> AddVegetableElementWithValidation(long vegSortId, double x, double y, List<MapElementDTO> existingElements);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleTreesAsync(long treeId);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleVegetablesAsync(long vegId);
        BaseResponseDTO<bool> ValidateElementPlacement(MapElementDTO element, List<MapElementDTO> existingElements);

        double CalculateDistance(double x1, double y1, double x2, double y2);
    }
}