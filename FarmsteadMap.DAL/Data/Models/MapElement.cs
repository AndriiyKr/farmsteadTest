// <copyright file="MapElement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a map element entity placed on a map.
    /// </summary>
    public class MapElement
    {
        /// <summary>
        /// Gets or sets the type of the element (e.g., "Tree", "Vegetable").
        /// </summary>
        required public string Type { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the specific element instance.
        /// </summary>
        required public long ElementId { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the element.
        /// </summary>
        required public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the element.
        /// </summary>
        required public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        required public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the element.
        /// </summary>
        required public double Height { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle of the element in degrees.
        /// </summary>
        required public int Rotation { get; set; }
    }
}