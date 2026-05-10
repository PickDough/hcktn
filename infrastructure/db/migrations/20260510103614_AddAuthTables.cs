using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace hcktn.infrastructure.db.migrations
{
    /// <inheritdoc />
    public partial class AddAuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationCredentials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganisationId = table.Column<long>(type: "bigint", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationCredentials_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationCredentials_OrganisationId",
                table: "OrganisationCredentials",
                column: "OrganisationId");

            migrationBuilder.Sql(@"
INSERT INTO ""Admins"" (""Login"", ""PasswordHash"") VALUES
    ('admin', '$2a$11$6dRZIJ6Xf4N6u2H98wXDEuye/00i9fmjnxCC27N/CDFmBcNOYH062');
");

            migrationBuilder.Sql(@"
INSERT INTO ""OrganisationCredentials"" (""OrganisationId"", ""Login"", ""PasswordHash"") VALUES
    (1, 'org1', '$2a$11$EsMP5bbDHD/Uqg35J/1Am.zgOS0ssyIExkmkISwBnS/Ls6oSv0qv2'),
    (2, 'org2', '$2a$11$xpiGYzFLWo/95PIwwY2vq.WPmnTBLJyWBCVcNV1jimvB.UXq/5ETW'),
    (3, 'org3', '$2a$11$5OnDMaRx98GkQ6PDgkTROeDXXfTIpUUiEJLqYeIiS4jLbsHpgJgxG'),
    (4, 'org4', '$2a$11$UbGVfUrkP0BFA/eK4Rma6O7MwprWEyOd2oBkg/FreV5byYylHb5MC'),
    (5, 'org5', '$2a$11$/gMu6EVGocdrMY0jmMSwc.c8LRUuT/dW/CWyj6l5TpJj3VwNiG1w2');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "OrganisationCredentials");
        }
    }
}
