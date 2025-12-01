// <copyright file="Vegetable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a vegetable entity in the database.
    /// </summary>
    public class Vegetable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vegetable.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the vegetable.
        /// </summary>
        required public string Name { get; set; }
    }
}