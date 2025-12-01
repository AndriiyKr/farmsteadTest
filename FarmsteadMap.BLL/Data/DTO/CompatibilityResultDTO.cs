<<<<<<< HEAD
﻿// <copyright file="CompatibilityResultDTO.cs" company="PlaceholderCompany">
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
=======
﻿namespace FarmsteadMap.BLL.Data.DTO
{
    public class CompatibilityResultDTO
    {
        public bool IsCompatible { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
