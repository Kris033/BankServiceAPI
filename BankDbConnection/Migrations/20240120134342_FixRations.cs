using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankDbConnection.Migrations
{
    /// <inheritdoc />
    public partial class FixRations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "contract_id",
                table: "employee");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "employee",
                newName: "passport_id");

            migrationBuilder.RenameColumn(
                name: "PassportId",
                table: "client",
                newName: "passport_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_currency_id",
                table: "employee",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_passport_id",
                table: "employee",
                column: "passport_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contract_employee_id",
                table: "contract",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_passport_id",
                table: "client",
                column: "passport_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_client_passport_passport_id",
                table: "client",
                column: "passport_id",
                principalTable: "passport",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_employee_employee_id",
                table: "contract",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_currency_currency_id",
                table: "employee",
                column: "currency_id",
                principalTable: "currency",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_passport_passport_id",
                table: "employee",
                column: "passport_id",
                principalTable: "passport",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_client_passport_passport_id",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_employee_employee_id",
                table: "contract");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_currency_currency_id",
                table: "employee");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_passport_passport_id",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_currency_id",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_passport_id",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_contract_employee_id",
                table: "contract");

            migrationBuilder.DropIndex(
                name: "IX_client_passport_id",
                table: "client");

            migrationBuilder.RenameColumn(
                name: "passport_id",
                table: "employee",
                newName: "PassportId");

            migrationBuilder.RenameColumn(
                name: "passport_id",
                table: "client",
                newName: "PassportId");

            migrationBuilder.AddColumn<Guid>(
                name: "contract_id",
                table: "employee",
                type: "uuid",
                nullable: true);

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
    }
}
