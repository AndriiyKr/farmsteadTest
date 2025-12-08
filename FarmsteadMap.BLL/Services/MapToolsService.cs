// <copyright file="MapToolsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides tools for managing map elements, measurements, and placements.
    /// </summary>
    public class MapToolsService : IMapToolsService
    {
        private readonly IMapRepository mapRepository;
        private readonly ICompatibilityService compatibilityService;
        private readonly ILoggerService logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapToolsService"/> class.
        /// </summary>
        /// <param name="mapRepository">The repository for map data.</param>
        /// <param name="compatibilityService">The service for checking element compatibility.</param>
        public MapToolsService(IMapRepository mapRepository, ICompatibilityService compatibilityService, ILoggerService logger)
        {
            this.mapRepository = mapRepository;
            this.compatibilityService = compatibilityService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public MapElementDTO AddTreeElement(long treeSortId, double x, double y) => new ()
        {
            Type = "tree",
            ElementId = treeSortId,
            X = x,
            Y = y,
            Width = 50,
            Height = 50,
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MapElementDTO AddVegetableElement(long vegSortId, double x, double y) => new ()
        {
            Type = "veg",
            ElementId = vegSortId,
            X = x,
            Y = y,
            Width = 30,
            Height = 30,
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MapElementDTO AddFlowerElement(long flowerId, double x, double y) => new ()
        {
            Type = "flower",
            ElementId = flowerId,
            X = x,
            Y = y,
            Width = 20,
            Height = 20,
            Rotation = 0,
        };

        /// <inheritdoc/>
        public MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY) => new ()
        {
            StartX = startX,
            StartY = startY,
            EndX = endX,
            EndY = endY,
            Length = this.CalculateDistance(startX, startY, endX, endY),
        };

        /// <inheritdoc/>
        public GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape = "rectangle") => new ()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Shape = shape,
        };

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapElementDTO>> AddTreeElementWithValidation(long treeSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
                var element = this.AddTreeElement(treeSortId, x, y);
                var validationResult = await this.compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                if (validationResult.Success)
                {
                    this.logger.LogEvent($"Додано дерево {treeSortId} на координати {x}:{y}");
                    return BaseResponseDTO<MapElementDTO>.Ok(element);
                }
                else
                {
                    this.logger.LogWarning($"Не вдалося додати дерево: {validationResult.Error}");
                    return BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error ?? "Validation failed");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Критична помилка при додаванні дерева", ex);
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні дерева: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<MapElementDTO>> AddVegetableElementWithValidation(long vegSortId, double x, double y, List<MapElementDTO> existingElements)
        {
            try
            {
                var element = this.AddVegetableElement(vegSortId, x, y);
                var validationResult = await this.compatibilityService.ValidateElementPlacementAsync(element, existingElements);

                if (validationResult.Success)
                {
                    this.logger.LogEvent($"Додано овоч {vegSortId} на координати {x}:{y}");
                    return BaseResponseDTO<MapElementDTO>.Ok(element);
                }
                else
                {
                    this.logger.LogWarning($"Не вдалося додати овоч: {validationResult.Error}");
                    return BaseResponseDTO<MapElementDTO>.Fail(validationResult.Error ?? "Validation failed");
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError("Критична помилка при додаванні овоча", ex);
                return BaseResponseDTO<MapElementDTO>.Fail($"Помилка при додаванні овоча: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleTreesAsync(long treeId)
        {
            try
            {
                var result = await this.compatibilityService.GetIncompatibleTreeIdsAsync(treeId);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка при отриманні несумісних дерев", ex);
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних дерев: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<List<long>>> GetIncompatibleVegetablesAsync(long vegId)
        {
            try
            {
                var result = await this.compatibilityService.GetIncompatibleVegIdsAsync(vegId);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка при отриманні несумісних овочів", ex);
                return BaseResponseDTO<List<long>>.Fail($"Помилка при отриманні несумісних овочів: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public BaseResponseDTO<bool> ValidateElementPlacement(MapElementDTO element, List<MapElementDTO> existingElements)
        {
            try
            {
                foreach (var existing in existingElements)
                {
                    if (this.DoElementsOverlap(element, existing))
                    {
                        this.logger.LogWarning("Спроба розмістити елемент на зайняте місце.");
                        return BaseResponseDTO<bool>.Fail("Елементи перекриваються");
                    }
                }

                return BaseResponseDTO<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Помилка алгоритму перевірки перекриття", ex);
                return BaseResponseDTO<bool>.Fail($"Помилка перевірки розміщення: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public double CalculateDistance(double x1, double y1, double x2, double y2) =>
            Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        private bool DoElementsOverlap(MapElementDTO a, MapElementDTO b) =>
            a.X < b.X + b.Width &&
            a.X + a.Width > b.X &&
            a.Y < b.Y + b.Height &&
            a.Y + a.Height > b.Y;
    }
}