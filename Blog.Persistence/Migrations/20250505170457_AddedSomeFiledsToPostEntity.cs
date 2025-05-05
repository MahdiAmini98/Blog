using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeFiledsToPostEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Posts");
        }
    }
}
