// <copyright file="MapShortDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    using System;

    /// <summary>
    /// Data Transfer Object representing a short summary of a map.
    /// </summary>
    public class MapShortDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the map.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is private.
        /// </summary>
        required public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the map was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}