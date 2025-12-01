// <copyright file="IMapToolsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the contract for a service that provides tools for managing map elements, measurements, and validation.
    /// </summary>
    public interface IMapToolsService
    {
        /// <summary>
        /// Creates a new tree map element.
        /// </summary>
        /// <param name="treeSortId">The identifier of the tree sort.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>A <see cref="MapElementDTO"/> representing the tree.</returns>
        MapElementDTO AddTreeElement(long treeSortId, double x, double y);

        /// <summary>
        /// Creates a new vegetable map element.
        /// </summary>
        /// <param name="vegSortId">The identifier of the vegetable sort.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>A <see cref="MapElementDTO"/> representing the vegetable.</returns>
        MapElementDTO AddVegetableElement(long vegSortId, double x, double y);

        /// <summary>
        /// Creates a new flower map element.
        /// </summary>
        /// <param name="flowerId">The identifier of the flower.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>A <see cref="MapElementDTO"/> representing the flower.</returns>
        MapElementDTO AddFlowerElement(long flowerId, double x, double y);

        /// <summary>
        /// Creates a new measurement line.
        /// </summary>
        /// <param name="startX">The starting X coordinate.</param>
        /// <param name="startY">The starting Y coordinate.</param>
        /// <param name="endX">The ending X coordinate.</param>
        /// <param name="endY">The ending Y coordinate.</param>
        /// <returns>A <see cref="MeasurementLineDTO"/> representing the measurement.</returns>
        MeasurementLineDTO AddMeasurement(double startX, double startY, double endX, double endY);

        /// <summary>
        /// Creates a new garden bed.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="width">The width of the garden bed.</param>
        /// <param name="height">The height of the garden bed.</param>
        /// <param name="shape">The shape of the garden bed.</param>
        /// <returns>A <see cref="GardenBedDTO"/> representing the garden bed.</returns>
        GardenBedDTO AddGardenBed(double x, double y, double width, double height, string shape);

        /// <summary>
        /// Adds a tree element with placement validation.
        /// </summary>
        /// <param name="treeSortId">The identifier of the tree sort.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="existingElements">The list of existing map elements.</param>
        /// <returns>A response containing the created element if validation succeeds.</returns>
        Task<BaseResponseDTO<MapElementDTO>> AddTreeElementWithValidation(long treeSortId, double x, double y, List<MapElementDTO> existingElements);

        /// <summary>
        /// Adds a vegetable element with placement validation.
        /// </summary>
        /// <param name="vegSortId">The identifier of the vegetable sort.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="existingElements">The list of existing map elements.</param>
        /// <returns>A response containing the created element if validation succeeds.</returns>
        Task<BaseResponseDTO<MapElementDTO>> AddVegetableElementWithValidation(long vegSortId, double x, double y, List<MapElementDTO> existingElements);

        /// <summary>
        /// Retrieves a list of incompatible trees for the specified tree ID.
        /// </summary>
        /// <param name="treeId">The identifier of the tree.</param>
        /// <returns>A response containing a list of incompatible tree IDs.</returns>
        Task<BaseResponseDTO<List<long>>> GetIncompatibleTreesAsync(long treeId);

        /// <summary>
        /// Retrieves a list of incompatible vegetables for the specified vegetable ID.
        /// </summary>
        /// <param name="vegId">The identifier of the vegetable.</param>
        /// <returns>A response containing a list of incompatible vegetable IDs.</returns>
        Task<BaseResponseDTO<List<long>>> GetIncompatibleVegetablesAsync(long vegId);

        /// <summary>
        /// Validates whether an element can be placed without overlapping existing elements.
        /// </summary>
        /// <param name="element">The element to place.</param>
        /// <param name="existingElements">The list of existing map elements.</param>
        /// <returns>A response indicating whether the placement is valid.</returns>
        BaseResponseDTO<bool> ValidateElementPlacement(MapElementDTO element, List<MapElementDTO> existingElements);

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="x1">The X coordinate of the first point.</param>
        /// <param name="y1">The Y coordinate of the first point.</param>
        /// <param name="x2">The X coordinate of the second point.</param>
        /// <param name="y2">The Y coordinate of the second point.</param>
        /// <returns>The calculated distance.</returns>
        double CalculateDistance(double x1, double y1, double x2, double y2);
    }
}