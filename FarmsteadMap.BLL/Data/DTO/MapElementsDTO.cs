<<<<<<< HEAD
﻿// <copyright file="MapElementsDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Data Transfer Object containing lists of available map elements (trees, vegetables, flowers).
    /// </summary>
    public class MapElementsDTO
    {
        /// <summary>
        /// Gets or sets the list of available tree sorts.
        /// </summary>
        public List<TreeSortDTO> TreeSorts { get; set; } = new List<TreeSortDTO>();

        /// <summary>
        /// Gets or sets the list of available vegetable sorts.
        /// </summary>
        public List<VegSortDTO> VegSorts { get; set; } = new List<VegSortDTO>();

        /// <summary>
        /// Gets or sets the list of available flowers.
        /// </summary>
        public List<FlowerDTO> Flowers { get; set; } = new List<FlowerDTO>();
    }
}
=======
﻿using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class MapElementsDTO
    {
        public List<TreeSortDTO> TreeSorts { get; set; } = new();
        public List<VegSortDTO> VegSorts { get; set; } = new();
        public List<FlowerDTO> Flowers { get; set; } = new();
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
