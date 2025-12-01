// <copyright file="CompatibilityResultDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing the result of a compatibility check between map elements.
    /// </summary>
    public class CompatibilityResultDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the elements are compatible.
        /// </summary>
        required public bool IsCompatible { get; set; }

        /// <summary>
        /// Gets or sets the message describing the compatibility result.
        /// </summary>
        required public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the result (e.g., "info", "warning", "error").
        /// </summary>
        required public string Severity { get; set; }
    }
}