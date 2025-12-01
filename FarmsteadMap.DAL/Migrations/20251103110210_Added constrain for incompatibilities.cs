<<<<<<< HEAD
﻿// <copyright file="20251103110210_Added constrain for incompatibilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
=======
﻿using Microsoft.EntityFrameworkCore.Migrations;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3

#nullable disable

namespace FarmsteadMap.DAL.Migrations
{
<<<<<<< HEAD
    using Microsoft.EntityFrameworkCore.Migrations;

=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
    /// <inheritdoc />
    public partial class Addedconstrainforincompatibilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "veg_incompatibility_check",
                table: "veg_incompatibility",
                sql: "veg1_id < veg2_id");

            migrationBuilder.AddCheckConstraint(
                name: "trees_incompatibility_check",
                table: "trees_incompatibility",
                sql: "tree1_id < tree2_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "veg_incompatibility_check",
                table: "veg_incompatibility");

            migrationBuilder.DropCheckConstraint(
                name: "trees_incompatibility_check",
                table: "trees_incompatibility");
        }
    }
}
