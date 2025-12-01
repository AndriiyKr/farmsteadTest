<<<<<<< HEAD
﻿// <copyright file="MapToolsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Repositories;

    /// <summary>
    /// Provides tools for managing map elements, measurements, and placements.
    /// </summary>
    public class MapToolsService : IMapToolsService
    {
        private readonly IMapRepository mapRepository;
        private readonly ICompatibilityService compatibilityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapToolsService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data.</param>
        /// <param name="compatibilityService">The service for checking element compatibility.</param>
        public MapToolsService(IMapRepository mapRepository, ICompatibilityService compatibilityService)
        {
            this.mapRepository = mapRepository;
            this.compatibilityService = compatibilityService;
        }

        /// <inheritdoc/>
        public MapElementDTO AddTreeElement(long treeSortId, double x, double y) => new ()
=======
﻿using FarmsteadMap.BLL.Data.DTO;
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        {
            Type = "tree",
            ElementId = treeSortId,
            X = x,
            Y = y,
            Width = 50,
            Height = 50,
<<<<<<< HEAD
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MapElementDTO AddVegetableElement(long vegSortId, double x, double y) => new ()
=======
            Rotation = 0
        };

        public MapElementDTO AddVegetableElement(long vegSortId, double x, double y) => new()
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        {
            Type = "veg",
            ElementId = vegSortId,
            X = x,
            Y = y,
            Width = 30,
            Height = 30,
<<<<<<< HEAD
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MapElementDTO AddFlowerElement(long flowerId, double x, double y) => new ()
=======
            Rotation = 0
        };

        public MapElementDTO AddFlowerElement(long flowerId, double x, double y) => new()
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        {
            Type = "flower",
            ElementId = flowerId,
            X = x,
            Y = y,
            Width = 20,
            Height = 20,
<<<<<<< HEAD
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY) => new ()
=======
            Rotation = 0
        };

        public MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY) => new()
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        {
            StartX = startX,
            StartY = startY,
            EndX = endX,
            EndY = endY,
<<<<<<< HEAD
            Length = this.CalculateDistance(startX, startY, endX, endY),
        };

        /// <inheritdoc/>
        public GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape = "rectangle") => new ()
=======
            Length = CalculateDistance(startX, startY, endX, endY)
        };

        public GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape = "rectangle") => new()
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
<<<<<<< HEAD
            Shape = shape,
        };

        /// <inheritdoc/>
=======
            Shape = shape
        };

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<MapElementDTO>> AddTreeElementWithValidation(long treeSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
<<<<<<< HEAD
                var element = this.AddTreeElement(treeSortId, x, y);
                var validationResult = await this.compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error ?? "Validation failed");
=======
                var element = AddTreeElement(treeSortId, x, y);
                var validationResult = await _compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні дерева: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<MapElementDTO>> AddVegetableElementWithValidation(long vegSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
<<<<<<< HEAD
                var element = this.AddVegetableElement(vegSortId, x, y);
                var validationResult = await this.compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error ?? "Validation failed");
=======
                var element = AddVegetableElement(vegSortId, x, y);
                var validationResult = await _compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                return validationResult.Success
                    ? BaseResponseDTO<MapElementDTO>.Ok(element)
                    : BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні овоча: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreesAsync(long treeId)
        {
            try
            {
<<<<<<< HEAD
                var result = await this.compatibilityService.GetIncompatibleTreeIdsAsync(treeId);
                return result;
=======
                var result = await _compatibilityService.GetIncompatibleTreeIdsAsync(treeId);
                return result; // Тепер повертаємо BaseResponseDTO<List<long>> напряму
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних дерев: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegetablesAsync(long vegId)
        {
            try
            {
<<<<<<< HEAD
                var result = await this.compatibilityService.GetIncompatibleVegIdsAsync(vegId);
                return result;
=======
                var result = await _compatibilityService.GetIncompatibleVegIdsAsync(vegId);
                return result; // Тепер повертаємо BaseResponseDTO<List<long>> напряму
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних овочів: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public BaseResponseDTO<bool> ValidateElementPlacement(MapElementDTO element, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existing in existingElements)
                {
<<<<<<< HEAD
                    if (this.DoElementsOverlap(element, existing))
                    {
                        return BaseResponseDTO<bool>.Fail("Елементи перекриваються");
                    }
                }

=======
                    if (DoElementsOverlap(element, existing))
                        return BaseResponseDTO<bool>.Fail("Елементи перекриваються");
                }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<bool>.Fail($"Помилка перевірки розміщення: {ex.Message}");
            }
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public double CalculateDistance(double x1, double y1, double x2, double y2) =>
            Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        private bool DoElementsOverlap(MapElementDTO a, MapElementDTO b) =>
<<<<<<< HEAD
            a.X < b.X + b.Width &&
            a.X + a.Width > b.X &&
            a.Y < b.Y + b.Height &&
            a.Y + a.Height > b.Y;
=======
            a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
    }
}