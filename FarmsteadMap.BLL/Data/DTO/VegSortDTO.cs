// <copyright file="VegSortDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a specific sort (variety) of a vegetable.
    /// </summary>
    public class VegSortDTO
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
        /// Gets or sets the name of the parent vegetable type.
        /// </summary>
        required public string VegetableName { get; set; }
    }
}