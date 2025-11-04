using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Repositories;

namespace FarmsteadMap.BLL.Services
{
    public class CompatibilityService : ICompatibilityService
    {
        private readonly IMapRepository _mapRepository;

        // Константи радіусів у метрах
        private const double TREE_INCOMPATIBILITY_RADIUS = 5.0;
        private const double VEG_INCOMPATIBILITY_RADIUS = 1.0;

        public CompatibilityService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existingElement in existingElements)
                {
                    var compatibility = await CheckCompatibilityAsync(newElement, existingElement);
                    if (!compatibility.Success)
                        return BaseResponseDTO<bool>.Fail(compatibility.Error);

                    if (!compatibility.Data.IsCompatible)
                    {
                        return BaseResponseDTO<bool>.Fail(compatibility.Data.Message);
                    }
                }
                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<bool>.Fail("Помилка перевірки розміщення елемента");
            }
        }

        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId)
        {
            try
            {
                var result = await _mapRepository.GetIncompatibleTreeIdsAsync(treeId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних дерев");
            }
        }

        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId)
        {
            try
            {
                var result = await _mapRepository.GetIncompatibleVegIdsAsync(vegId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних овочів");
            }
        }

        public async Task<BaseResponseDTO<CompatibilityResultDTO>> CheckCompatibilityAsync(MapElementDTO element1, MapElementDTO element2)
        {
            try
            {
                // Перевірка для дерев
                if (element1.Type == "tree" && element2.Type == "tree")
                {
                    var tree1Name = await GetTreeNameAsync(element1.ElementId);
                    var tree2Name = await GetTreeNameAsync(element2.ElementId);

                    if (AreElementsWithinDistance(element1, element2, TREE_INCOMPATIBILITY_RADIUS))
                    {
                        var areIncompatible = await _mapRepository.AreTreesIncompatibleAsync(element1.ElementId, element2.ElementId);
                        if (areIncompatible)
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' несумісні у радіусі {TREE_INCOMPATIBILITY_RADIUS} метрів",
                                Severity = "warning"
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' сумісні",
                                Severity = "info"
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
                            Message = $"Дерева '{tree1Name}' та '{tree2Name}' не знаходяться у радіусі {TREE_INCOMPATIBILITY_RADIUS} метрів",
                            Severity = "info"
                        });
                    }
                }

                // Перевірка для овочів
                if (element1.Type == "veg" && element2.Type == "veg")
                {
                    var veg1Name = await GetVegetableNameAsync(element1.ElementId);
                    var veg2Name = await GetVegetableNameAsync(element2.ElementId);

                    if (AreElementsWithinDistance(element1, element2, VEG_INCOMPATIBILITY_RADIUS))
                    {
                        var areIncompatible = await _mapRepository.AreVegetablesIncompatibleAsync(element1.ElementId, element2.ElementId);
                        if (areIncompatible)
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' несумісні у радіусі {VEG_INCOMPATIBILITY_RADIUS} метрів",
                                Severity = "warning"
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' сумісні",
                                Severity = "info"
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
                            Message = $"Овочі '{veg1Name}' та '{veg2Name}' не знаходяться у радіусі {VEG_INCOMPATIBILITY_RADIUS} метрів",
                            Severity = "info"
                        });
                    }
                }

                // Для різних типів рослин або інших комбінацій
                return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                {
                    IsCompatible = true,
                    Message = "Різні типи рослин",
                    Severity = "info"
                });
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<CompatibilityResultDTO>.Fail("Помилка перевірки сумісності");
            }
        }

        private bool AreElementsWithinDistance(MapElementDTO element1, MapElementDTO element2, double maxDistanceMeters)
        {
            try
            {
                var center1 = new PointDTO(
                    element1.X + element1.Width / 2,
                    element1.Y + element1.Height / 2
                );
                var center2 = new PointDTO(
                    element2.X + element2.Width / 2,
                    element2.Y + element2.Height / 2
                );

                var distance = CalculateDistance(center1, center2);
                return distance <= maxDistanceMeters;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private double CalculateDistance(PointDTO point1, PointDTO point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        private async Task<string> GetTreeNameAsync(long treeSortId)
        {
            try
            {
                var treeSorts = await _mapRepository.GetTreeSortsAsync();
                var treeSort = treeSorts.FirstOrDefault(ts => ts.Id == treeSortId);
                return treeSort?.Name ?? $"Tree#{treeSortId}";
            }
            catch (Exception ex)
            {
                return $"Tree#{treeSortId}";
            }
        }

        private async Task<string> GetVegetableNameAsync(long vegSortId)
        {
            try
            {
                var vegSorts = await _mapRepository.GetVegSortsAsync();
                var vegSort = vegSorts.FirstOrDefault(vs => vs.Id == vegSortId);
                return vegSort?.Name ?? $"Vegetable#{vegSortId}";
            }
            catch (Exception ex)
            {
                return $"Vegetable#{vegSortId}";
            }
        }
    }
}