using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymDivision.Migrations
{
    /// <inheritdoc />
    public partial class updateModelForPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PairMember",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PairMember",
                table: "Members");
        }
    }
}
