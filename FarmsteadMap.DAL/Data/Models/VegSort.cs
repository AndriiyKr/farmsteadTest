// <copyright file="VegSort.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a specific sort (variety) of a vegetable entity.
    /// </summary>
    public class VegSort
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vegetable sort.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the vegetable sort.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the image of the vegetable sort.
        /// </summary>
        required public string Image { get; set; }

        /// <summary>
        /// Gets or sets the type of ground suitable for this vegetable sort.
        /// </summary>
        required public string GroundType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent vegetable type.
        /// </summary>
        required public long VegId { get; set; }

        /// <summary>
        /// Gets or sets the parent vegetable entity.
        /// </summary>
        required public Vegetable Vegetable { get; set; }
    }
}