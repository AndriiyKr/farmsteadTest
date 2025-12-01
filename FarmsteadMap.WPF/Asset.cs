// <copyright file="Asset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    /// <summary>
    /// Представляє об'єкт (ассет) на панелі інструментів.
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Gets or sets ID об'єкта в базі даних.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets назву об'єкта.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets шлях до іконки об'єкта.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets тип об'єкта (tree, building тощо).
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}