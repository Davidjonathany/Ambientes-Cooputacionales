using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalVentasMVC.Migrations
{
    /// <inheritdoc />
    public partial class ClienteCorreo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Cliente");
        }
    }
}
