// <copyright file="MeasurementLineDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a measurement line on the map.
    /// </summary>
    public class MeasurementLineDTO
    {
        /// <summary>
        /// Gets or sets the X coordinate of the starting point of the line.
        /// </summary>
        public double StartX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the starting point of the line.
        /// </summary>
        public double StartY { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the ending point of the line.
        /// </summary>
        public double EndX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the ending point of the line.
        /// </summary>
        public double EndY { get; set; }

        /// <summary>
        /// Gets or sets the calculated length of the measurement line.
        /// </summary>
        public double Length { get; set; }
    }
}