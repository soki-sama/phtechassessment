﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Propeller.DALC.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCustomerVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EMail = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultLocale = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Locale = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CustomerStatusID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerStatuses_CustomerStatusID",
                        column: x => x.CustomerStatusID,
                        principalTable: "CustomerStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactCustomer",
                columns: table => new
                {
                    ContactsID = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomersID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCustomer", x => new { x.ContactsID, x.CustomersID });
                    table.ForeignKey(
                        name: "FK_ContactCustomer_Contacts_ContactsID",
                        column: x => x.ContactsID,
                        principalTable: "Contacts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactCustomer_Customers_CustomersID",
                        column: x => x.CustomersID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notes_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "CountryCode", "DefaultLocale", "Name" },
                values: new object[,]
                {
                    { new Guid("2a28502a-abed-4c01-91f7-414f09e94edd"), "NZL", "en-NZ", "New Zealand" },
                    { new Guid("9fb03922-2c31-4e70-9291-96653d599aa5"), "MEX", "es-MX", "Mexico" },
                    { new Guid("a843203b-552f-41c8-b052-9a52f767b681"), "FRA", "fr-FR", "France" }
                });

            migrationBuilder.InsertData(
                table: "CustomerStatuses",
                columns: new[] { "ID", "State" },
                values: new object[,]
                {
                    { 1, "prospective" },
                    { 2, "current" },
                    { 3, "non-active" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Locale", "Name", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "en-NZ", "English Administrator", "s3cUrE.p4s5W0Rd.1", 99, "admin.en@mail.com" },
                    { 2, "en-NZ", "English User", "s3cUrE.p4s5W0Rd.2", 1, "user.en@mail.com" },
                    { 3, "es-MX", "Administrador México", "s3cUrE.p4s5W0Rd.1", 99, "admin.es@mail.com" },
                    { 4, "es-MX", "Usuario México", "s3cUrE.p4s5W0Rd.2", 1, "user.es@mail.com" },
                    { 5, "fr-FR", "French Administrateur", "s3cUrE.p4s5W0Rd.1", 99, "admin.fr@mail.com" },
                    { 6, "fr-FR", "French Utilisateur", "s3cUrE.p4s5W0Rd.2", 1, "user.fr@mail.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactCustomer_CustomersID",
                table: "ContactCustomer",
                column: "CustomersID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerStatusID",
                table: "Customers",
                column: "CustomerStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CustomerID",
                table: "Notes",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactCustomer");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerStatuses");
        }
    }
}