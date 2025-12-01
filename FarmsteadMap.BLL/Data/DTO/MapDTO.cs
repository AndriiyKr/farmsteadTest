// <copyright file="MapDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a map entity.
    /// </summary>
    public class MapDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the map.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the JSON string representing the map structure.
        /// </summary>
        required public string MapJson { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is private.
        /// </summary>
        required public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who owns the map.
        /// </summary>
        required public long UserId { get; set; }
    }
}