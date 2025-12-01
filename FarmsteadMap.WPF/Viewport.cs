// <copyright file="Viewport.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using Newtonsoft.Json;

    /// <summary>
    /// Зберігає стан камери (zoom, offset) (для JSON серіалізації).
    /// </summary>
    public class Viewport
    {
        /// <summary>
        /// Gets or sets рівень масштабування.
        /// </summary>
        [JsonProperty("zoom")]
        public double Zoom { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets зсув по горизонталі.
        /// </summary>
        [JsonProperty("offsetX")]
        public double OffsetX { get; set; } = 0;

        /// <summary>
        /// Gets or sets зсув по вертикалі.
        /// </summary>
        [JsonProperty("offsetY")]
        public double OffsetY { get; set; } = 0;
    }
}