// <copyright file="FlowerDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a flower map element.
    /// </summary>
    public class FlowerDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the flower.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the flower.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of ground suitable for the flower.
        /// </summary>
        required public string GroundType { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the flower's image.
        /// </summary>
        required public string Image { get; set; }
    }
}