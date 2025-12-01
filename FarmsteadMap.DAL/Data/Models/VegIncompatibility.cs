<<<<<<< HEAD
﻿// <copyright file="VegIncompatibility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    }
}
=======
﻿namespace FarmsteadMap.DAL.Data.Models
{
    public class VegIncompatibility
    {
        public long Veg1Id { get; set; }
        public long Veg2Id { get; set; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
