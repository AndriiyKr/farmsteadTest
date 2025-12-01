// <copyright file="MapObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using Newtonsoft.Json;

    /// <summary>
    /// Представляє об'єкт, розміщений на мапі (для JSON серіалізації).
    /// </summary>
    public class MapObject
    {
        /// <summary>
        /// Gets or sets ID об'єкта з бази даних.
        /// </summary>
        [JsonProperty("db_id")]
        public long DbId { get; set; }

        /// <summary>
        /// Gets or sets тип об'єкта.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets X-координату.
        /// </summary>
        [JsonProperty("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets Y-координату.
        /// </summary>
        [JsonProperty("y")]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets кут обертання.
        /// </summary>
        [JsonProperty("rotation")]
        public double Rotation { get; set; }

        /// <summary>
        /// Gets or sets ширину.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets висоту.
        /// </summary>
        public double Height { get; set; }
    }
}