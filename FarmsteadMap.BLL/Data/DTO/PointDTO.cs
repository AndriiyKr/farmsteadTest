<<<<<<< HEAD
﻿// <copyright file="PointDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a 2D point with X and Y coordinates.
    /// </summary>
    public class PointDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointDTO"/> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public PointDTO(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public double Y { get; set; }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class PointDTO
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PointDTO(double x, double y) { X = x; Y = y; }
    }
}
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
