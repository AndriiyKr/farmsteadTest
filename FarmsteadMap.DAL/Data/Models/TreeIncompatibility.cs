<<<<<<< HEAD
﻿// <copyright file="TreeIncompatibility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents an incompatibility rule between two tree types.
    /// </summary>
    public class TreeIncompatibility
    {
        /// <summary>
        /// Gets or sets the unique identifier of the first tree involved in the incompatibility.
        /// </summary>
        public long Tree1Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the second tree involved in the incompatibility.
        /// </summary>
        public long Tree2Id { get; set; }
    }
}
=======
﻿namespace FarmsteadMap.DAL.Data.Models
{
    public class TreeIncompatibility
    {
        public long Tree1Id { get; set; }
        public long Tree2Id { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
