// <copyright file="NavigateEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;

    /// <summary>
    /// Містить дані для подій, що запитують навігацію між View.
    /// </summary>
    public class NavigateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets назву (ключ) View, куди потрібно перейти.
        /// </summary>
        public string TargetView { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets (необов'язково) дані, які потрібно передати у View.
        /// </summary>
        public object? Data { get; set; }
    }
}