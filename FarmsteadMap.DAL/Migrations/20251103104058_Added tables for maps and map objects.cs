<<<<<<< HEAD
﻿// <copyright file="20251103104058_Added tables for maps and map objects.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
=======
﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3

#nullable disable

namespace FarmsteadMap.DAL.Migrations
{
<<<<<<< HEAD
    using Microsoft.EntityFrameworkCore.Migrations;
    using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
    /// <inheritdoc />
    public partial class Addedtablesformapsandmapobjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flowers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ground_type = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
<<<<<<< HEAD
                    image = table.Column<string>(type: "text", nullable: false),
=======
                    image = table.Column<string>(type: "text", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flowers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    map = table.Column<string>(type: "text", nullable: false),
                    is_Private = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
<<<<<<< HEAD
                    user_id = table.Column<long>(type: "bigint", nullable: false),
=======
                    user_id = table.Column<long>(type: "bigint", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => x.id);
                    table.ForeignKey(
                        name: "map_user_fk",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
<<<<<<< HEAD
                    image = table.Column<string>(type: "text", nullable: false),
=======
                    image = table.Column<string>(type: "text", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vegetables",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
<<<<<<< HEAD
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
=======
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vegetables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tree_sorts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ground_type = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
<<<<<<< HEAD
                    tree_id = table.Column<long>(type: "bigint", nullable: false),
=======
                    tree_id = table.Column<long>(type: "bigint", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tree_sorts", x => x.id);
                    table.ForeignKey(
                        name: "sorts_tree_fk",
                        column: x => x.tree_id,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trees_incompatibility",
                columns: table => new
                {
                    tree1_id = table.Column<long>(type: "bigint", nullable: false),
<<<<<<< HEAD
                    tree2_id = table.Column<long>(type: "bigint", nullable: false),
=======
                    tree2_id = table.Column<long>(type: "bigint", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trees_incompatibility", x => new { x.tree1_id, x.tree2_id });
                    table.ForeignKey(
                        name: "incompatibility_tree1_fk",
                        column: x => x.tree1_id,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "incompatibility_tree2_fk",
                        column: x => x.tree2_id,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "veg_incompatibility",
                columns: table => new
                {
                    veg1_id = table.Column<long>(type: "bigint", nullable: false),
<<<<<<< HEAD
                    veg2_id = table.Column<long>(type: "bigint", nullable: false),
=======
                    veg2_id = table.Column<long>(type: "bigint", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veg_incompatibility", x => new { x.veg1_id, x.veg2_id });
                    table.ForeignKey(
                        name: "incompatibility_veg1_fk",
                        column: x => x.veg1_id,
                        principalTable: "vegetables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "incompatibility_veg2_fk",
                        column: x => x.veg2_id,
                        principalTable: "vegetables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "veg_sorts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    ground_type = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
<<<<<<< HEAD
                    veg_id = table.Column<long>(type: "bigint", nullable: false),
=======
                    veg_id = table.Column<long>(type: "bigint", nullable: false)
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veg_sorts", x => x.id);
                    table.ForeignKey(
                        name: "sorts_veg_fk",
                        column: x => x.veg_id,
                        principalTable: "vegetables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_maps_user_id",
                table: "maps",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tree_sorts_tree_id",
                table: "tree_sorts",
                column: "tree_id");

            migrationBuilder.CreateIndex(
                name: "IX_trees_incompatibility_tree2_id",
                table: "trees_incompatibility",
                column: "tree2_id");

            migrationBuilder.CreateIndex(
                name: "IX_veg_incompatibility_veg2_id",
                table: "veg_incompatibility",
                column: "veg2_id");

            migrationBuilder.CreateIndex(
                name: "IX_veg_sorts_veg_id",
                table: "veg_sorts",
                column: "veg_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flowers");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "tree_sorts");

            migrationBuilder.DropTable(
                name: "trees_incompatibility");

            migrationBuilder.DropTable(
                name: "veg_incompatibility");

            migrationBuilder.DropTable(
                name: "veg_sorts");

            migrationBuilder.DropTable(
                name: "trees");

            migrationBuilder.DropTable(
                name: "vegetables");
        }
    }
}
