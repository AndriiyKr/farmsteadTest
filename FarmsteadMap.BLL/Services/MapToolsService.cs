using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Repositories;

namespace FarmsteadMap.BLL.Services
{
    public class MapToolsService : IMapToolsService
    {
        private readonly IMapRepository _mapRepository;
        private readonly ICompatibilityService _compatibilityService;

        public MapToolsService(IMapRepository mapRepository, ICompatibilityService compatibilityService)
        {
            _mapRepository = mapRepository;
            _compatibilityService = compatibilityService;
        }

        public MapElementDTO AddTreeElement(long treeSortId, double x, double y) => new()
        {
            Type = "tree",
            ElementId = treeSortId,
            X = x,
            Y = y,
            Width = 50,
            Height = 50,
            Rotation = 0
        };

        public MapElementDTO AddVegetableElement(long vegSortId, double x, double y) => new()
        {
            Type = "veg",
            ElementId = vegSortId,
            X = x,
            Y = y,
            Width = 30,
            Height = 30,
            Rotation = 0
        };

        public MapElementDTO AddFlowerElement(long flowerId, double x, double y) => new()
        {
            Type = "flower",
            ElementId = flowerId,
            X = x,
            Y = y,
            Width = 20,
            Height = 20,
            Rotation = 0
        };

        public MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY) => new()
        {
            StartX = startX,
            StartY = startY,
            EndX = endX,
            EndY = endY,
            Length = CalculateDistance(startX, startY, endX, endY)
        };

        public GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape = "rectangle") => new()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Shape = shape
        };

        public async Task<BaseResponseDTO<MapElementDTO>> AddTreeElementWithValidation(long treeSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
                var element = AddTreeElement(treeSortId, x, y);
                var validationResult = await _compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні дерева: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<MapElementDTO>> AddVegetableElementWithValidation(long vegSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
                var element = AddVegetableElement(vegSortId, x, y);
                var validationResult = await _compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні овоча: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreesAsync(long treeId)
        {
            try
            {
                var result = await _compatibilityService.GetIncompatibleTreeIdsAsync(treeId);
                return result; // Тепер повертаємо BaseResponseDTO<List<long>> напряму
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних дерев: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegetablesAsync(long vegId)
        {
            try
            {
                var result = await _compatibilityService.GetIncompatibleVegIdsAsync(vegId);
                return result; // Тепер повертаємо BaseResponseDTO<List<long>> напряму
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних овочів: {ex.Message}");
            }
        }

        public BaseResponseDTO<bool> ValidateElementPlacement(MapElementDTO element, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existing in existingElements)
                {
                    if (DoElementsOverlap(element, existing))
                        return BaseResponseDTO<bool>.Fail("Елементи перекриваються");
                }
                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<bool>.Fail($"Помилка перевірки розміщення: {ex.Message}");
            }
        }

        public double CalculateDistance(double x1, double y1, double x2, double y2) =>
            Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        private bool DoElementsOverlap(MapElementDTO a, MapElementDTO b) =>
            a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y;
    }
}