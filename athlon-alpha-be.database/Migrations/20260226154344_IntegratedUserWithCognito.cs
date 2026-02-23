using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace athlon_alpha_be.database.Migrations;

/// <inheritdoc />
public partial class IntegratedUserWithCognito : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CognitoSub",
            table: "User",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CognitoSub",
            table: "User");
    }
}
