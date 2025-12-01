<<<<<<< HEAD
﻿// <copyright file="TreeSort.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a specific sort (variety) of a tree entity.
    /// </summary>
    public class TreeSort
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
        /// Gets or sets the parent tree entity.
        /// </summary>
        required public Tree Tree { get; set; }
    }
}
=======
﻿namespace FarmsteadMap.DAL.Data.Models
{
    public class TreeSort
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroundType { get; set; }
        public long TreeId { get; set; }
        public Tree Tree { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
