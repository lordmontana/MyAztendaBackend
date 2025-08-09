using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocationService.Migrations
{
    /// <inheritdoc />
    public partial class Initiallocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "atzenda");

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "atzenda",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    crop_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    crop_type_id = table.Column<int>(type: "integer", nullable: true),
                    address_line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address_line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    postcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    elevation_meters = table.Column<double>(type: "double precision", nullable: true),
                    contact_person = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    parent_location_id = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    i_id = table.Column<int>(type: "integer", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_location", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_location_i_id",
                schema: "atzenda",
                table: "Location",
                column: "i_id");

            migrationBuilder.CreateIndex(
                name: "ix_location_name",
                schema: "atzenda",
                table: "Location",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Location",
                schema: "atzenda");
        }
    }
}
