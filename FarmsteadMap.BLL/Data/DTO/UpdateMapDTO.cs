// <copyright file="UpdateMapDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for updating an existing map.
    /// </summary>
    public class UpdateMapDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the map to update.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the new name of the map.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is private.
        /// </summary>
        required public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the updated data content of the map.
        /// </summary>
        required public MapDataDTO MapData { get; set; }
    }
}