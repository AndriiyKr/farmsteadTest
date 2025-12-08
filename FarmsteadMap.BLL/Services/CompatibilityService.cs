// <copyright file="CompatibilityService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Repositories;

    /// <summary>
    /// Service responsible for checking compatibility between map elements (trees, vegetables).
    /// </summary>
    public class CompatibilityService : ICompatibilityService
    {
        // Constants for incompatibility radii in meters
        private const double TreeIncompatibilityRadius = 5.0;
        private const double VegIncompatibilityRadius = 1.0;

        private readonly IMapRepository mapRepository;
        private readonly ILoggerService logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompatibilityService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data.</param>
        public CompatibilityService(IMapRepository mapRepository, ILoggerService logger)
        {
            this.mapRepository = mapRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existingElement in existingElements)
                {
                    var compatibility = await this.CheckCompatibilityAsync(newElement, existingElement);

                    if (!compatibility.Success)
                    {
                        // Fix CS8604: Ensure error string is never null
                        return BaseResponseDTO<bool>.Fail(compatibility.Error ?? "Unknown compatibility check error");
                    }

                    // Check Data for null before accessing properties
                    if (compatibility.Data != null && !compatibility.Data.IsCompatible)
                    {
                        this.logger.LogWarning($"Валідація розміщення: {compatibility.Data.Message}");
                        return BaseResponseDTO<bool>.Fail(compatibility.Data.Message);
                    }
                }

                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка перевірки розміщення елемента", ex);
                return BaseResponseDTO<bool>.Fail("Помилка перевірки розміщення елемента");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId)
        {
            try
            {
                var result = await this.mapRepository.GetIncompatibleTreeIdsAsync(treeId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка отримання несумісних дерев", ex);
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних дерев");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId)
        {
            try
            {
                var result = await this.mapRepository.GetIncompatibleVegIdsAsync(vegId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка отримання несумісних овочів", ex);
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних овочів");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<CompatibilityResultDTO>> CheckCompatibilityAsync(MapElementDTO element1, MapElementDTO element2)
        {
            try
            {
                // Перевірка для дерев
                if (element1.Type == "tree" && element2.Type == "tree")
                {
                    var tree1Name = await this.GetTreeNameAsync(element1.ElementId);
                    var tree2Name = await this.GetTreeNameAsync(element2.ElementId);

                    if (this.AreElementsWithinDistance(element1, element2, TreeIncompatibilityRadius))
                    {
                        var areIncompatible = await this.mapRepository.AreTreesIncompatibleAsync(element1.ElementId, element2.ElementId);
                        if (areIncompatible)
                        {
                            this.logger.LogWarning($"Виявлено несумісність: {tree1Name} та {tree2Name}");
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' несумісні у радіусі {TreeIncompatibilityRadius} метрів",
                                Severity = "warning",
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' сумісні",
                                Severity = "info",
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
                            Message = $"Дерева '{tree1Name}' та '{tree2Name}' не знаходяться у радіусі {TreeIncompatibilityRadius} метрів",
                            Severity = "info",
                        });
                    }
                }

                // Перевірка для овочів
                if (element1.Type == "veg" && element2.Type == "veg")
                {
                    var veg1Name = await this.GetVegetableNameAsync(element1.ElementId);
                    var veg2Name = await this.GetVegetableNameAsync(element2.ElementId);

                    if (this.AreElementsWithinDistance(element1, element2, VegIncompatibilityRadius))
                    {
                        var areIncompatible = await this.mapRepository.AreVegetablesIncompatibleAsync(element1.ElementId, element2.ElementId);
                        if (areIncompatible)
                        {
                            this.logger.LogWarning($"Виявлено несумісність: {veg1Name} та {veg2Name}");
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' несумісні у радіусі {VegIncompatibilityRadius} метрів",
                                Severity = "warning",
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' сумісні",
                                Severity = "info",
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
                            Message = $"Овочі '{veg1Name}' та '{veg2Name}' не знаходяться у радіусі {VegIncompatibilityRadius} метрів",
                            Severity = "info",
                        });
                    }
                }

                // Для різних типів рослин або інших комбінацій
                return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                {
                    IsCompatible = true,
                    Message = "Різні типи рослин",
                    Severity = "info",
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка перевірки сумісності", ex);
                return BaseResponseDTO<CompatibilityResultDTO>.Fail("Помилка перевірки сумісності");
            }
        }

        private bool AreElementsWithinDistance(MapElementDTO element1, MapElementDTO element2, double maxDistanceMeters)
        {
            try
            {
                var center1 = new PointDTO(
                    element1.X + (element1.Width / 2),
                    element1.Y + (element1.Height / 2));
                var center2 = new PointDTO(
                    element2.X + (element2.Width / 2),
                    element2.Y + (element2.Height / 2));

                var distance = this.CalculateDistance(center1, center2);
                return distance <= maxDistanceMeters;
            }
            catch (Exception)
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
                var treeSorts = await this.mapRepository.GetTreeSortsAsync();
                var treeSort = treeSorts.FirstOrDefault(ts => ts.Id == treeSortId);
                return treeSort?.Name ?? $"Tree#{treeSortId}";
            }
            catch (Exception)
            {
                return $"Tree#{treeSortId}";
            }
        }

        private async Task<string> GetVegetableNameAsync(long vegSortId)
        {
            try
            {
                var vegSorts = await this.mapRepository.GetVegSortsAsync();
                var vegSort = vegSorts.FirstOrDefault(vs => vs.Id == vegSortId);
                return vegSort?.Name ?? $"Vegetable#{vegSortId}";
            }
            catch (Exception)
            {
                return $"Vegetable#{vegSortId}";
            }
        }
    }
}