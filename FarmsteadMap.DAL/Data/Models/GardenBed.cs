// <copyright file="GardenBed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a garden bed entity in the database.
    /// </summary>
    public class GardenBed
    {
        /// <summary>
        /// Gets or sets the X coordinate of the garden bed.
        /// </summary>
        required public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the garden bed.
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
        /// Gets or sets the shape of the garden bed (e.g., "rectangle", "circle").
        /// </summary>
        required public string Shape { get; set; }
    }
}