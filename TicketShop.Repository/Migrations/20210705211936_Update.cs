using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketShop.Web.Data.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Tickets_OrderId",
                table: "TicketsInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Orders_TicketId",
                table: "TicketsInOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInOrder_Orders_OrderId",
                table: "TicketsInOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInOrder_Tickets_TicketId",
                table: "TicketsInOrder",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Orders_OrderId",
                table: "TicketsInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInOrder_Tickets_TicketId",
                table: "TicketsInOrder");

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
    }
}
