using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityAPI.Migrations
{
    public partial class data_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Manchester of Pakistan", "Faisalabad" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 2, "Second biggest city of Pakistan", "Lahore" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, "Biggest city of Pakistan", "Karachi" });

            migrationBuilder.InsertData(
                table: "Pois",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 1, 1, "Center of the city connecting 8 major centers", "Clock Tower" });

            migrationBuilder.InsertData(
                table: "Pois",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 2, 1, "Canal road's oldest subrub", "Madina Town" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Pois",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pois",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
