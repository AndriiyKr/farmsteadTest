// <copyright file="Map.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    /// <summary>
    /// Представляє модель даних для мапи.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Gets or sets ID мапи.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets назву мапи.
        /// </summary>
        public string Name { get; set; } = string.Empty; // Виправлено CS8618

        /// <summary>
        /// Gets or sets URL зображення мапи.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty; // Виправлено CS8618
    }
}