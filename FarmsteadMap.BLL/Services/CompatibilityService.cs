<<<<<<< HEAD
﻿// <copyright file="CompatibilityService.cs" company="PlaceholderCompany">
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CompatibilityService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data.</param>
        public CompatibilityService(IMapRepository mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        /// <inheritdoc/>
=======
﻿using FarmsteadMap.BLL.Data.DTO;
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

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existingElement in existingElements)
                {
<<<<<<< HEAD
                    var compatibility = await this.CheckCompatibilityAsync(newElement, existingElement);

                    if (!compatibility.Success)
                    {
                        // Fix CS8604: Ensure error string is never null
                        return BaseResponseDTO<bool>.Fail(compatibility.Error ?? "Unknown compatibility check error");
                    }

                    // Check Data for null before accessing properties
                    if (compatibility.Data != null && !compatibility.Data.IsCompatible)
=======
                    var compatibility = await CheckCompatibilityAsync(newElement, existingElement);
                    if (!compatibility.Success)
                        return BaseResponseDTO<bool>.Fail(compatibility.Error);

                    if (!compatibility.Data.IsCompatible)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                    {
                        return BaseResponseDTO<bool>.Fail(compatibility.Data.Message);
                    }
                }
<<<<<<< HEAD

                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception)
=======
                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<bool>.Fail("Помилка перевірки розміщення елемента");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId)
        {
            try
            {
<<<<<<< HEAD
                var result = await this.mapRepository.GetIncompatibleTreeIdsAsync(treeId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception)
=======
                var result = await _mapRepository.GetIncompatibleTreeIdsAsync(treeId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних дерев");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId)
        {
            try
            {
<<<<<<< HEAD
                var result = await this.mapRepository.GetIncompatibleVegIdsAsync(vegId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception)
=======
                var result = await _mapRepository.GetIncompatibleVegIdsAsync(vegId);
                return BaseResponseDTO<List<long>>.Ok(result);
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<List<long>>.Fail("Помилка отримання несумісних овочів");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<CompatibilityResultDTO>> CheckCompatibilityAsync(MapElementDTO element1, MapElementDTO element2)
        {
            try
            {
                // Перевірка для дерев
                if (element1.Type == "tree" && element2.Type == "tree")
                {
<<<<<<< HEAD
                    var tree1Name = await this.GetTreeNameAsync(element1.ElementId);
                    var tree2Name = await this.GetTreeNameAsync(element2.ElementId);

                    if (this.AreElementsWithinDistance(element1, element2, TreeIncompatibilityRadius))
                    {
                        var areIncompatible = await this.mapRepository.AreTreesIncompatibleAsync(element1.ElementId, element2.ElementId);
=======
                    var tree1Name = await GetTreeNameAsync(element1.ElementId);
                    var tree2Name = await GetTreeNameAsync(element2.ElementId);

                    if (AreElementsWithinDistance(element1, element2, TREE_INCOMPATIBILITY_RADIUS))
                    {
                        var areIncompatible = await _mapRepository.AreTreesIncompatibleAsync(element1.ElementId, element2.ElementId);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                        if (areIncompatible)
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
<<<<<<< HEAD
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' несумісні у радіусі {TreeIncompatibilityRadius} метрів",
                                Severity = "warning",
=======
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' несумісні у радіусі {TREE_INCOMPATIBILITY_RADIUS} метрів",
                                Severity = "warning"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Дерева '{tree1Name}' та '{tree2Name}' сумісні",
<<<<<<< HEAD
                                Severity = "info",
=======
                                Severity = "info"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
<<<<<<< HEAD
                            Message = $"Дерева '{tree1Name}' та '{tree2Name}' не знаходяться у радіусі {TreeIncompatibilityRadius} метрів",
                            Severity = "info",
=======
                            Message = $"Дерева '{tree1Name}' та '{tree2Name}' не знаходяться у радіусі {TREE_INCOMPATIBILITY_RADIUS} метрів",
                            Severity = "info"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                        });
                    }
                }

                // Перевірка для овочів
                if (element1.Type == "veg" && element2.Type == "veg")
                {
<<<<<<< HEAD
                    var veg1Name = await this.GetVegetableNameAsync(element1.ElementId);
                    var veg2Name = await this.GetVegetableNameAsync(element2.ElementId);

                    if (this.AreElementsWithinDistance(element1, element2, VegIncompatibilityRadius))
                    {
                        var areIncompatible = await this.mapRepository.AreVegetablesIncompatibleAsync(element1.ElementId, element2.ElementId);
=======
                    var veg1Name = await GetVegetableNameAsync(element1.ElementId);
                    var veg2Name = await GetVegetableNameAsync(element2.ElementId);

                    if (AreElementsWithinDistance(element1, element2, VEG_INCOMPATIBILITY_RADIUS))
                    {
                        var areIncompatible = await _mapRepository.AreVegetablesIncompatibleAsync(element1.ElementId, element2.ElementId);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                        if (areIncompatible)
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = false,
<<<<<<< HEAD
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' несумісні у радіусі {VegIncompatibilityRadius} метрів",
                                Severity = "warning",
=======
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' несумісні у радіусі {VEG_INCOMPATIBILITY_RADIUS} метрів",
                                Severity = "warning"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                            });
                        }
                        else
                        {
                            return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                            {
                                IsCompatible = true,
                                Message = $"Овочі '{veg1Name}' та '{veg2Name}' сумісні",
<<<<<<< HEAD
                                Severity = "info",
=======
                                Severity = "info"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                            });
                        }
                    }
                    else
                    {
                        return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                        {
                            IsCompatible = true,
<<<<<<< HEAD
                            Message = $"Овочі '{veg1Name}' та '{veg2Name}' не знаходяться у радіусі {VegIncompatibilityRadius} метрів",
                            Severity = "info",
=======
                            Message = $"Овочі '{veg1Name}' та '{veg2Name}' не знаходяться у радіусі {VEG_INCOMPATIBILITY_RADIUS} метрів",
                            Severity = "info"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                        });
                    }
                }

                // Для різних типів рослин або інших комбінацій
                return BaseResponseDTO<CompatibilityResultDTO>.Ok(new CompatibilityResultDTO
                {
                    IsCompatible = true,
                    Message = "Різні типи рослин",
<<<<<<< HEAD
                    Severity = "info",
                });
            }
            catch (Exception)
=======
                    Severity = "info"
                });
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return BaseResponseDTO<CompatibilityResultDTO>.Fail("Помилка перевірки сумісності");
            }
        }

        private bool AreElementsWithinDistance(MapElementDTO element1, MapElementDTO element2, double maxDistanceMeters)
        {
            try
            {
                var center1 = new PointDTO(
<<<<<<< HEAD
                    element1.X + (element1.Width / 2),
                    element1.Y + (element1.Height / 2));
                var center2 = new PointDTO(
                    element2.X + (element2.Width / 2),
                    element2.Y + (element2.Height / 2));

                var distance = this.CalculateDistance(center1, center2);
                return distance <= maxDistanceMeters;
            }
            catch (Exception)
=======
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
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
<<<<<<< HEAD
                var treeSorts = await this.mapRepository.GetTreeSortsAsync();
                var treeSort = treeSorts.FirstOrDefault(ts => ts.Id == treeSortId);
                return treeSort?.Name ?? $"Tree#{treeSortId}";
            }
            catch (Exception)
=======
                var treeSorts = await _mapRepository.GetTreeSortsAsync();
                var treeSort = treeSorts.FirstOrDefault(ts => ts.Id == treeSortId);
                return treeSort?.Name ?? $"Tree#{treeSortId}";
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return $"Tree#{treeSortId}";
            }
        }

        private async Task<string> GetVegetableNameAsync(long vegSortId)
        {
            try
            {
<<<<<<< HEAD
                var vegSorts = await this.mapRepository.GetVegSortsAsync();
                var vegSort = vegSorts.FirstOrDefault(vs => vs.Id == vegSortId);
                return vegSort?.Name ?? $"Vegetable#{vegSortId}";
            }
            catch (Exception)
=======
                var vegSorts = await _mapRepository.GetVegSortsAsync();
                var vegSort = vegSorts.FirstOrDefault(vs => vs.Id == vegSortId);
                return vegSort?.Name ?? $"Vegetable#{vegSortId}";
            }
            catch (Exception ex)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            {
                return $"Vegetable#{vegSortId}";
            }
        }
    }
}