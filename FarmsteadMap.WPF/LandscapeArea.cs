// <copyright file="LandscapeArea.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System.Collections.Generic;
    using System.Windows;
    using Newtonsoft.Json;

    /// <summary>
    /// Представляє зону ландшафту (для JSON серіалізації).
    /// </summary>
    public class LandscapeArea
    {
        /// <summary>
        /// Gets or sets тип зони (трава, грунт тощо).
        /// </summary>
        [JsonProperty(nameof(AreaType))]
        public string AreaType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets список точок, що формують полігон.
        /// </summary>
        [JsonProperty(nameof(Points))]
        public List<Point> Points { get; set; } = new List<Point>();
    }
}