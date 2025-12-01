<<<<<<< HEAD
﻿// <copyright file="MapDataDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Data Transfer Object representing the detailed data structure of a map.
    /// </summary>
    public class MapDataDTO
    {
        /// <summary>
        /// Gets or sets the name associated with the map data.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of elements (trees, plants, etc.) placed on the map.
        /// </summary>
        public List<MapElementDTO> Elements { get; set; } = new ();

        /// <summary>
        /// Gets or sets the list of measurement lines drawn on the map.
        /// </summary>
        public List<MeasurementLineDTO> Measurements { get; set; } = new ();

        /// <summary>
        /// Gets or sets the list of garden beds defined on the map.
        /// </summary>
        public List<GardenBedDTO> GardenBeds { get; set; } = new ();
    }
}
=======
﻿using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapDataDTO
    {
        public string Name { get; set; }
        public List<MapElementDTO> Elements { get; set; } = new();
        public List<MeasurementLineDTO> Measurements { get; set; } = new();
        public List<GardenBedDTO> GardenBeds { get; set; } = new();
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
