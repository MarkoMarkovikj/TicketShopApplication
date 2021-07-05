using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketShop.Web.Data.Migrations
{
    public partial class TableNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Tickets_OrderId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_TicketId",
                table: "ProductInOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrders",
                table: "ProductInOrders");

            migrationBuilder.RenameTable(
                name: "ProductInOrders",
                newName: "TicketsInOrder");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_TicketId",
                table: "TicketsInOrder",
                newName: "IX_TicketsInOrder_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_OrderId",
                table: "TicketsInOrder",
                newName: "IX_TicketsInOrder_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketsInOrder",
                table: "TicketsInOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInOrder_Tickets_OrderId",
                table: "TicketsInOrder",
                column: "OrderId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInOrder_Orders_TicketId",
                table: "TicketsInOrder",
                column: "TicketId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Tickets_OrderId",
                table: "TicketsInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Orders_TicketId",
                table: "TicketsInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketsInOrder",
                table: "TicketsInOrder");

            migrationBuilder.RenameTable(
                name: "TicketsInOrder",
                newName: "ProductInOrders");

            migrationBuilder.RenameIndex(
                name: "IX_TicketsInOrder_TicketId",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketsInOrder_OrderId",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrders",
                table: "ProductInOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Tickets_OrderId",
                table: "ProductInOrders",
                column: "OrderId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_TicketId",
                table: "ProductInOrders",
                column: "TicketId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
