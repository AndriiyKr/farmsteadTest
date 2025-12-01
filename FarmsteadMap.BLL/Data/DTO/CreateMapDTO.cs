// <copyright file="CreateMapDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for creating a new map.
    /// </summary>
    public class CreateMapDTO
    {
        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is private.
        /// </summary>
        required public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user creating the map.
        /// </summary>
        required public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the initial data content of the map.
        /// </summary>
        required public MapDataDTO MapData { get; set; }
    }
}