<<<<<<< HEAD
﻿// <copyright file="Tree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a tree entity in the database.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tree.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tree species.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the image of the tree.
        /// </summary>
        required public string Image { get; set; }
    }
}
=======
﻿namespace FarmsteadMap.DAL.Data.Models
{
    public class Tree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
