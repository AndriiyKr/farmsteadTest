<<<<<<< HEAD
﻿// <copyright file="MapShortDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    using System;

    /// <summary>
    /// Data Transfer Object representing a short summary of a map.
    /// </summary>
    public class MapShortDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the map.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map is private.
        /// </summary>
        required public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the map was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
=======
﻿namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapShortDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
