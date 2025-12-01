<<<<<<< HEAD
﻿// <copyright file="TreeSortDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a specific sort (variety) of a tree.
    /// </summary>
    public class TreeSortDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tree sort.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tree sort.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of ground suitable for this tree sort.
        /// </summary>
        required public string GroundType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent tree species.
        /// </summary>
        required public long TreeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent tree species.
        /// </summary>
        required public string TreeName { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the parent tree species image.
        /// </summary>
        required public string TreeImage { get; set; }
    }
}
=======
﻿namespace FarmsteadMap.BLL.Data.DTO
{
    public class TreeSortDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroundType { get; set; }
        public long TreeId { get; set; }
        public string TreeName { get; set; }
        public string TreeImage { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
