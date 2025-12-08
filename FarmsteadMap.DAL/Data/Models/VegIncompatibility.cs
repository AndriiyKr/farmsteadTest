// <copyright file="VegIncompatibility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations.Schema;

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents an incompatibility rule between two vegetable types.
    /// </summary>
    public class VegIncompatibility
    {
        /// <summary>
        /// Gets or sets the unique identifier of the first vegetable involved in the incompatibility.
        /// </summary>
        public long Veg1Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the second vegetable involved in the incompatibility.
        /// </summary>
        public long Veg2Id { get; set; }

        [ForeignKey("Veg1Id")]
        public virtual Vegetable Veg1 { get; set; }

        [ForeignKey("Veg2Id")]
        public virtual Vegetable Veg2 { get; set; }
    }
}