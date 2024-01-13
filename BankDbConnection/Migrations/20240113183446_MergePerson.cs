using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankDbConnection.Migrations
{
    /// <inheritdoc />
    public partial class MergePerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_person_id",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_person_id",
                table: "employee");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "employee",
                newName: "PassportId");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "client",
                newName: "PassportId");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "employee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "in_black_list",
                table: "employee",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "employee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "number_phone",
                table: "employee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "client",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "in_black_list",
                table: "client",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "client",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "number_phone",
                table: "client",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_employee_PassportId",
                table: "employee",
                column: "PassportId");

            migrationBuilder.CreateIndex(
                name: "IX_client_PassportId",
                table: "client",
                column: "PassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_passport_PassportId",
                table: "client",
                column: "PassportId",
                principalTable: "passport",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_passport_PassportId",
                table: "employee",
                column: "PassportId",
                principalTable: "passport",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_passport_PassportId",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_passport_PassportId",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_PassportId",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_client_PassportId",
                table: "client");

            migrationBuilder.DropColumn(
                name: "age",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "in_black_list",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "name",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "number_phone",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "age",
                table: "client");

            migrationBuilder.DropColumn(
                name: "in_black_list",
                table: "client");

            migrationBuilder.DropColumn(
                name: "name",
                table: "client");

            migrationBuilder.DropColumn(
                name: "number_phone",
                table: "client");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "employee",
                newName: "person_id");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "client",
                newName: "person_id");

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassportId = table.Column<Guid>(type: "uuid", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    in_black_list = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    number_phone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_passport_PassportId",
                        column: x => x.PassportId,
                        principalTable: "passport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_person_PassportId",
                table: "person",
                column: "PassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_client_person_id",
                table: "client",
                column: "id",
                principalTable: "person",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_person_id",
                table: "employee",
                column: "id",
                principalTable: "person",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
