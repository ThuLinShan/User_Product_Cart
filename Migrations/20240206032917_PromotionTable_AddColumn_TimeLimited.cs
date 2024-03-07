using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Product_Cart.Migrations
{
    /// <inheritdoc />
    public partial class PromotionTable_AddColumn_TimeLimited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "timeLimited",
                table: "Promotions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeLimited",
                table: "Promotions");
        }
    }
}
