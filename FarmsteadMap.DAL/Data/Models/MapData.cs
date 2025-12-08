// <copyright file="MapData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the detailed data structure of a map stored in JSON format.
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// Gets or sets the name of the map configuration.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of elements placed on the map.
        /// </summary>
        public List<MapElement> Elements { get; set; } = new List<MapElement>();

        /// <summary>
        /// Gets or sets the list of measurement lines on the map.
        /// </summary>
        public List<MeasurementLine> Measurements { get; set; } = new List<MeasurementLine>();

        /// <summary>
        /// Gets or sets the list of garden beds on the map.
        /// </summary>
        public List<GardenBed> GardenBeds { get; set; } = new List<GardenBed>();

        /// <summary>
        /// Gets or sets the date and time when the map data was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the map data was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}