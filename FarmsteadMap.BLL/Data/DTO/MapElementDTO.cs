// <copyright file="MapElementDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing an element placed on the map.
    /// </summary>
    public class MapElementDTO
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
        /// Gets or sets the X coordinate of the element's position.
        /// </summary>
        required public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the element's position.
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