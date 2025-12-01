// <copyright file="MeasurementLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a measurement line entity used for measuring distances on the map.
    /// </summary>
    public class MeasurementLine
    {
        /// <summary>
        /// Gets or sets the X coordinate of the starting point.
        /// </summary>
        public double StartX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the starting point.
        /// </summary>
        public double StartY { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the ending point.
        /// </summary>
        public double EndX { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the ending point.
        /// </summary>
        public double EndY { get; set; }

        /// <summary>
        /// Gets or sets the calculated length of the measurement line.
        /// </summary>
        public double Length { get; set; }
    }
}