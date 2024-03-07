using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Product_Cart.Migrations
{
    /// <inheritdoc />
    public partial class Added_Event_Modify_Product2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventProduct_Event_EventId",
                table: "EventProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EventProduct_Products_ProductId",
                table: "EventProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventProduct",
                table: "EventProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "EventProduct",
                newName: "EventsProduct");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_EventProduct_ProductId",
                table: "EventsProduct",
                newName: "IX_EventsProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventsProduct",
                table: "EventsProduct",
                columns: new[] { "EventId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsProduct_Events_EventId",
                table: "EventsProduct",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventsProduct_Products_ProductId",
                table: "EventsProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsProduct_Events_EventId",
                table: "EventsProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EventsProduct_Products_ProductId",
                table: "EventsProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventsProduct",
                table: "EventsProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "EventsProduct",
                newName: "EventProduct");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameIndex(
                name: "IX_EventsProduct_ProductId",
                table: "EventProduct",
                newName: "IX_EventProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventProduct",
                table: "EventProduct",
                columns: new[] { "EventId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventProduct_Event_EventId",
                table: "EventProduct",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventProduct_Products_ProductId",
                table: "EventProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
