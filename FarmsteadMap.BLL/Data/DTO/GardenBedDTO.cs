// <copyright file="GardenBedDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a garden bed on the map.
    /// </summary>
    public class GardenBedDTO
    {
        /// <summary>
        /// Gets or sets the X coordinate of the garden bed's position.
        /// </summary>
        required public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the garden bed's position.
        /// </summary>
        required public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the garden bed.
        /// </summary>
        required public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the garden bed.
        /// </summary>
        required public double Height { get; set; }

        /// <summary>
        /// Gets or sets the geometric shape of the garden bed (e.g., "rectangle", "circle").
        /// </summary>
        required public string Shape { get; set; }
    }
}