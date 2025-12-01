// <copyright file="MapScene.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Головний клас-контейнер для всієї сцени мапи (для JSON серіалізації).
    /// </summary>
    public class MapScene
    {
        /// <summary>
        /// Gets or sets стан камери.
        /// </summary>
        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; } = new Viewport();

        /// <summary>
        /// Gets or sets список об'єктів на мапі.
        /// </summary>
        [JsonProperty("objects")]
        public List<MapObject> Objects { get; set; } = new List<MapObject>();

        /// <summary>
        /// Gets or sets список зон ландшафту.
        /// </summary>
        [JsonProperty("landscape")]
        public List<LandscapeArea> Landscape { get; set; } = new List<LandscapeArea>();
    }
}