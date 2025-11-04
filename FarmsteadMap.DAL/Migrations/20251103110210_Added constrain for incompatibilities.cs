using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmsteadMap.DAL.Migrations
{
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
