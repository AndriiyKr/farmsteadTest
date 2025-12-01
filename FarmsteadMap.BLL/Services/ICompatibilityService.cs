<<<<<<< HEAD
﻿// <copyright file="ICompatibilityService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the contract for a service that checks compatibility and placement rules for map elements.
    /// </summary>
    public interface ICompatibilityService
    {
        /// <summary>
        /// Validates whether a new element can be placed on the map alongside existing elements.
        /// </summary>
        /// <param name="newElement">The new element to be placed.</param>
        /// <param name="existingElements">The list of elements already present on the map.</param>
        /// <returns>A response containing a boolean indicating if the placement is valid.</returns>
        Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements);

        /// <summary>
        /// Retrieves a list of tree IDs that are incompatible with the specified tree.
        /// </summary>
        /// <param name="treeId">The identifier of the tree.</param>
        /// <returns>A response containing a list of incompatible tree IDs.</returns>
        Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId);

        /// <summary>
        /// Retrieves a list of vegetable IDs that are incompatible with the specified vegetable.
        /// </summary>
        /// <param name="vegId">The identifier of the vegetable.</param>
        /// <returns>A response containing a list of incompatible vegetable IDs.</returns>
        Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId);

        /// <summary>
        /// Checks the compatibility between two specific map elements.
        /// </summary>
        /// <param name="element1">The first map element.</param>
        /// <param name="element2">The second map element.</param>
        /// <returns>A response containing the detailed compatibility result.</returns>
=======
﻿using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface ICompatibilityService
    {
        Task<BaseResponseDTO<bool>> ValidateElementPlacementAsync(MapElementDTO newElement, List<MapElementDTO> existingElements);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleTreeIdsAsync(long treeId);
        Task<BaseResponseDTO<List<long>>> GetIncompatibleVegIdsAsync(long vegId);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        Task<BaseResponseDTO<CompatibilityResultDTO>> CheckCompatibilityAsync(MapElementDTO element1, MapElementDTO element2);
    }
}