// <copyright file="EditorTool.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    /// <summary>
    /// Визначає доступні інструменти в редакторі мапи.
    /// </summary>
    public enum EditorTool
    {
        /// <summary>
        /// Інструмент вибору (наразі не використовується).
        /// </summary>
        Select,

        /// <summary>
        /// Інструмент панорамування (перетягування мапи).
        /// </summary>
        Pan,

        /// <summary>
        /// Інструмент розміщення об'єкта.
        /// </summary>
        PlaceAsset,

        /// <summary>
        /// Інструмент малювання зони.
        /// </summary>
        DrawArea,

        /// <summary>
        /// Інструмент "Лінійка".
        /// </summary>
        Ruler,
    }
}