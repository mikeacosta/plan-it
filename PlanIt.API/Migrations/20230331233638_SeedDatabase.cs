using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlanIt.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Username" },
                values: new object[,]
                {
                    { 1, "joe@blow.com", "joeblow" },
                    { 2, "mickey@disney.com", "mickey.mouse" },
                    { 3, "user@gmail", "anonymous" }
                });

            migrationBuilder.InsertData(
                table: "Experiences",
                columns: new[] { "Id", "City", "Country", "Description", "State", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Amsterdam", "Netherlands", "Experience the beauty of Amsterdam’s canals by going on this scenic cruise. Pass by Anne Frank House, the Jordaan, the Houseboat Museum, Leiden Square, Rijksmuseum, De Duif and much more.", null, "Amsterdam Open Boat Canal Cruise - Live Guide - from Anne Frank House", 1 },
                    { 2, "Dubai", "United Arab Emirates", "Experience several desert pursuits in one outing, including ATV-driving—something many tours only offer at an extra cost—on this red-dune desert tour from Dubai. Skip the hassle of transport and logistical planning; and be free to simply enjoy the dunes and activities provided. Zoom off on an ATV, ride a camel, go sandboarding; enjoy henna art and Arabian-costume photos; and conclude with a barbecue-buffet dinner and live shows.", null, "Dubai: Red Dunes Quad Bike, Sandsurf, Camels & BBQ at Al Khayma Camp", 1 },
                    { 3, "Haleiwa", "United States", "Skip the hassle of renting a car and see the highlights of Oahu’s North Shore from the comfort of a minibus or van. Along the way, a guide keeps you entertained and shares details about the island that you would likely miss if traveling on your own. At each stop, you can enjoy free time to swim, shop, paddleboard/kayak or do an optional waterfall hike while getting to know the island.", "Hawaii", "Tour of North Shore and Sightseeing", 2 },
                    { 4, "London", "United Kingdom", "This historic castle with over 1,000 years of history is home to the Crown Jewels, the iconic 'Beefeater' Yeoman Warders, and the legendary ravens that have kept the kingdom from collapsing. Inside the White Tower, the oldest building of the castle, is an 11th-century chapel and historic Royal Armouries collections.", null, "Tower of London", 2 },
                    { 5, "Seoul", "South Korea", "The National Museum of Korea and the National Folk Museum are located on the grounds of this palace, built six centuries ago by the founder of the Chosun dynasty.", null, "Gyeongbokgung Palace", 3 },
                    { 6, "Marrakech", "Morocco", "Nicely designed and maintained gardens, similar to those of Generalife in Granada, Spain. It’s a good place to recoup from the intensity of the market atmosphere.", null, "Jardin Majorelle", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Experiences",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
