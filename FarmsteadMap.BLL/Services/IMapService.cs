<<<<<<< HEAD
﻿// <copyright file="IMapService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the service operations for managing maps.
    /// </summary>
    public interface IMapService
    {
        /// <summary>
        /// Retrieves a specific map by its identifier.
        /// </summary>
        /// <param name="mapId">The unique identifier of the map.</param>
        /// <returns>A response containing the map details.</returns>
        Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId);

        /// <summary>
        /// Retrieves all maps belonging to a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A response containing a list of short map summaries.</returns>
        Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId);

        /// <summary>
        /// Retrieves available map elements (trees, vegetables, flowers) for the map editor.
        /// </summary>
        /// <returns>A response containing the list of map elements.</returns>
        Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync();

        /// <summary>
        /// Creates a new map.
        /// </summary>
        /// <param name="createMapDto">The data required to create the map.</param>
        /// <returns>A response containing the identifier of the created map.</returns>
        Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto);

        /// <summary>
        /// Updates an existing map.
        /// </summary>
        /// <param name="updateMapDto">The data to update the map with.</param>
        /// <returns>A response indicating the success of the update operation.</returns>
        Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto);

        /// <summary>
        /// Deletes a map by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the map to delete.</param>
        /// <returns>A response indicating the success of the deletion operation.</returns>
=======
﻿using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface IMapService
    {
        Task<BaseResponseDTO<MapDTO>> GetMapAsync(long mapId);
        Task<BaseResponseDTO<List<MapShortDTO>>> GetUserMapsAsync(long userId);
        Task<BaseResponseDTO<MapElementsDTO>> GetMapElementsAsync();
        Task<BaseResponseDTO<long>> CreateMapAsync(CreateMapDTO createMapDto);
        Task<BaseResponseDTO<bool>> UpdateMapAsync(UpdateMapDTO updateMapDto);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        Task<BaseResponseDTO<bool>> DeleteMapAsync(long id);
    }
}