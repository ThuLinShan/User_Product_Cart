﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Product_Cart.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProdcutAddStockColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "Products");
        }
    }
}
